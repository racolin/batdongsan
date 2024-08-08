using Application.Common.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Domain;
using System.Net.Mail;
using System.Net;
using Domain.Constants;
using System.Web;
using Infrastructure.Identity;

namespace Infrastructure.Services;

public class MailService : IMailService
{
    private readonly SmtpConfig _configOptions;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IApplicationDbContext _context;

    public MailService
        (
            IOptions<ConfigOptions> configuration, 
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment webHostEnvironment, IApplicationDbContext context
        )
    {
        _configOptions = configuration.Value.Smtp;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
        _context = context;
    }

    private string? AppUrl
    {
        get
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
                return null;
            return $"{context.Request.Scheme}://{context.Request.Host}";
        }
    }

    public async Task<string?> SendRegister(string email, string image, string name, string description, string redirect)
    {
        redirect = $"{AppUrl}/{redirect}";
        image = $"{AppUrl}/{image}";

        var path = Path.Combine(_webHostEnvironment.WebRootPath, "MailTemplate", "SendRegister.html");
        var message = await File.ReadAllTextAsync(path);

        message = message.Replace("[[image]]", image);
        message = message.Replace("[[name]]", name);
        message = message.Replace("[[description]]", description);
        message = message.Replace("[[redirect]]", redirect);

        var result = await SendMail(email, DefaultConstant.WebName, message);
        return result;
    }

    public async Task<string?> SendAdminNewGuest(string email, string name, string? address, string phone, string content, DateTime time, string redirect)
    {
        redirect = $"{AppUrl}/{redirect}";

        var path = Path.Combine(_webHostEnvironment.WebRootPath, "MailTemplate", "SendAdminNewGuest.html");
        var message = await File.ReadAllTextAsync(path);

        message = message.Replace("[[name]]", name);
        message = message.Replace("[[address]]", address);
        message = message.Replace("[[phone]]", phone);
        message = message.Replace("[[content]]", content);
        message = message.Replace("[[time]]", time.ToString("HH:mm, dd/MM/yyyy"));
        message = message.Replace("[[redirect]]", redirect);

        var result = await SendMail(email, DefaultConstant.WebName, message);
        return result;
    }

    public async Task<string?> SendAdminResetPassword(string email, string username, string token, string fullname, string operation, string browser)
    {
        var redirect_url = $"{AppUrl}/admin/auth/resetpassword?token={token}&username={username}";

        var path = Path.Combine(_webHostEnvironment.WebRootPath, "MailTemplate", "SendResetPassword.html");
        var message = await File.ReadAllTextAsync(path);

        message = message
            .Replace("{{redirect_url}}", redirect_url)
            .Replace("{{fullname}}", fullname)
            .Replace("{{browser}}", browser)
            .Replace("{{operation}}", operation)
            .Replace("{{system_name}}", DefaultConstant.WebName)
            .Replace("{{system_logo}}", $"{AppUrl}{DefaultConstant.LogoLight}");

        var result = await SendMail(email, DefaultConstant.WebName, message);
        return result;
    }

    private async Task<string?> SendMail(string email, string subject, string body)
    {
        if (string.IsNullOrWhiteSpace(email))
            return "Mail is empty";

        var smtpMail = _configOptions.FromAddress;
        var smtpPassword = _configOptions.Password;

        var msg = new MailMessage();
        msg.To.Add(new MailAddress(email));
        msg.From = new MailAddress(smtpMail, _configOptions.FromDisplayName);
        msg.Subject = subject;
        msg.Body = body;
        msg.IsBodyHtml = true;

        var client = new SmtpClient();
        client.UseDefaultCredentials = _configOptions.DefaultCredential;
        client.Credentials = new NetworkCredential(smtpMail, smtpPassword);
        client.Port = _configOptions.Port; // You can use Port 25 if 587 is blocked (mine is!)
        client.Host = _configOptions.Host;
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.EnableSsl = _configOptions.EnableSSL;
        try
        {
            await client.SendMailAsync(msg);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }

        return null;
    }
}