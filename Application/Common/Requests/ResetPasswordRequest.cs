using Domain.Constants;
using FluentValidation;

namespace Application.Common.Requests
{
    public class ResetPasswordRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class ResetPasswordValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Tên đăng nhập không thể để trống!");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Mật khẩu mới không thể để trống!")
                .Matches(RegexConstant.Password).WithMessage("Mật khẩu không đúng định dạng!");

        }
    }
}
