namespace Application.Common.Interfaces;

public interface IMailService
{
    Task<string?> SendAdminContact(string receiveEmail, int id, string name, string email, string phone, string? address, string content);
    Task<string?> SendAdminResetPassword(string email, string username, string token, string fullname, string operation, string browser);
}