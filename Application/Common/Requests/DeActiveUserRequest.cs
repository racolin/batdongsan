using FluentValidation;

namespace Application.Common.Requests
{
    public class DeActiveUserRequest
    {
        public string? Username { get; set; }
    }

    public class DeActiveUserValidator : AbstractValidator<DeActiveUserRequest>
    {
        public DeActiveUserValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Tên đăng nhập không thể để trống!");
        }
    }
}