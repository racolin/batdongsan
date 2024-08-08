using FluentValidation;

namespace Application.Common.Requests
{
    public class RefreshTokenRequest
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
    public class RefreshTokenValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenValidator()
        {
            RuleFor(x => x.AccessToken).NotEmpty().WithMessage("Không thể làm mới token, hãy đăng nhập lại!");
            RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("Không thể làm mới token, hãy đăng nhập lại!");
        }
    }
}