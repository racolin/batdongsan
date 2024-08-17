using Application.Common.Mappings;
using Domain.Constants;
using Domain.Entities;
using FluentValidation;

namespace Application.Common.Requests
{
    public class SendRegisterMailRequest : IMapFrom<RegisterMailEntity>
    {
        public string Email { get; set; } = string.Empty;
        public string RecaptchaV3Token { get; set; } = string.Empty;
    }
    public class SendRegisterMailValidator : AbstractValidator<SendRegisterMailRequest>
    {
        public SendRegisterMailValidator()
        {

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email không thể để để trống!")
                .Matches(RegexConstant.Email).WithMessage("Email không đúng định dạng!");
        }
    }
}