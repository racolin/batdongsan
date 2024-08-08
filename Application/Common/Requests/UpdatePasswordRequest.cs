using Domain.Constants;
using FluentValidation;

namespace Application.Common.Requests
{
    public class UpdatePasswordRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
    public class UpdatePasswordValidator : AbstractValidator<UpdatePasswordRequest>
    {
        public UpdatePasswordValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Tên đăng nhập không thể để trống!");

            RuleFor(x => x.Password).Must((x, password) => {
                return password != x.NewPassword;
            }).WithMessage("Mật khẩu mới phải khác mật khẩu cũ!");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Mật khẩu không thể để trống!")
                .Matches(RegexConstant.Password).WithMessage("Mật khẩu không đúng định dạng!");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Mật khẩu mới không thể để trống!")
                .Matches(RegexConstant.Password).WithMessage("Mật khẩu mới không đúng định dạng!");
        }
    }
}
