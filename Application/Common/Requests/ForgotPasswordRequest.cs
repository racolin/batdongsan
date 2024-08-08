using FluentValidation;

namespace Application.Common.Requests
{
    public class ForgotPasswordRequest
    {
        public string? Username { get; set; }
        public bool IsResend { get; set; }
    }
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordRequest>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Tên đăng nhập không thể để trống!");
        }
    }
}
