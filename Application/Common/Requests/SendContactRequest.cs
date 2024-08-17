using Application.Common.Mappings;
using Domain.Constants;
using Domain.Entities;
using FluentValidation;

namespace Application.Common.Requests
{
    public class SendContactRequest : IMapFrom<ContactEntity>
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string RecaptchaV3Token { get; set; } = string.Empty;
    }
    public class SendContactValidator : AbstractValidator<SendContactRequest>
    {
        public SendContactValidator()
        {

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email không thể để để trống!")
                .Matches(RegexConstant.Email).WithMessage("Email không đúng định dạng!");

            RuleFor(x => x.Phone)
                .Matches(RegexConstant.Phone)
                .WithMessage("Số điện thoại không đúng định dạng!");

            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Nội dung không được để trống!");
        }
    }
}