namespace Application.Common.Interfaces;

public interface IMailService
{
    Task<string?> SendRegister(string email, string image, string name, string description, string redirect);
    Task<string?> SendAdminNewGuest(string email, string name, string? address, string phone, string content, DateTime time, string redirect);
    Task<string?> SendAdminResetPassword(string email, string username, string token, string fullname, string operation, string browser);
}