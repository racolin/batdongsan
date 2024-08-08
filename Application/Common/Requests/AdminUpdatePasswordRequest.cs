using Domain.Constants;
using FluentValidation;

namespace Application.Common.Requests
{
    public class AdminUpdatePasswordRequest
    {
        public int Id { get; set; }
        public string Password { get; set; } = string.Empty;
    }
    public class AdminUpdatePasswordValidator : AbstractValidator<AdminUpdatePasswordRequest>
    {
        public AdminUpdatePasswordValidator()
        {

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Mật khẩu không thể để trống!")
                .Matches(RegexConstant.Password).WithMessage("Mật khẩu không đúng định dạng!");

        }
    }
}
