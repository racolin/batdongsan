namespace Application.Common.Interfaces;

public interface IRecaptchaService
{
    Task<bool> ConfirmRecaptchaV3(string token, string? ip);
}