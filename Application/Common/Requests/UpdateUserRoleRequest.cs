using Domain.Constants;
using FluentValidation;

namespace Application.Common.Requests
{
    public class UpdateUserRoleRequest
    {
        public string? Username { get; set; }
        public string Role { get; set; } = string.Empty;
    }
    public class ChangeUserRoleValidator : AbstractValidator<UpdateUserRoleRequest>
    {
        public ChangeUserRoleValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Tên đăng nhập không thể để trống!");

            RuleFor(x => x.Role).Must((x, role) =>
            {
                return new List<string> {
                    RoleConstant.Admin,
                    RoleConstant.NewsPoster,
                }
                .Contains(role);
            }).WithMessage("Role này không tồn tại!");
        }
    }
}
