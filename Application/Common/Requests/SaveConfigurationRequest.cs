using Application.Common.Mappings;
using Domain.Constants;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;

namespace Application.Common.Requests
{
    public class SaveConfigurationRequest : IMapFrom<ConfigurationEntity>
    {
        public int? Id { get; set; }
        public string SendEmail { get; set; }
        public string SendEmailPassword { get; set; }
        public string ReceiveEmail { get; set; }
        public string ContactPhone { get; set; }
        public string ContactZaloPhone { get; set; }
        public string SystemPhone { get; set; }
        public string SystemEmail { get; set; }
        public string SystemZaloLink { get; set; }
        public string SystemYoutubeLink { get; set; }
        public string SystemFacebookLink { get; set; }
    }
    public class SaveConfigurationValidator : AbstractValidator<SaveConfigurationRequest>
    {
        public SaveConfigurationValidator()
        {
            RuleFor(x => x.SendEmail)
                .NotEmpty().WithMessage("SendEmail không thể để trống!")
                .Matches(RegexConstant.Email).WithMessage("SendEmail không đúng định dạng!");

            RuleFor(x => x.SendEmailPassword).NotEmpty().WithMessage("SendEmailPassword không thể để trống!");

            RuleFor(x => x.ReceiveEmail)
                .NotEmpty().WithMessage("ReceiveEmail không thể để trống!")
                .Matches(RegexConstant.Email).WithMessage("ReceiveEmail không đúng định dạng!");

            RuleFor(x => x.ContactPhone)
                .NotEmpty().WithMessage("ContactPhone không thể để trống!")
                .Matches(RegexConstant.Phone).WithMessage("ContactPhone không đúng định dạng!");

            RuleFor(x => x.ContactZaloPhone)
                .NotEmpty().WithMessage("ContactZaloPhone không thể để trống!")
                .Matches(RegexConstant.Phone).WithMessage("ContactZaloPhone không đúng định dạng!");

            RuleFor(x => x.SystemPhone)
                .NotEmpty().WithMessage("SystemPhone không thể để trống!")
                .Matches(RegexConstant.Phone).WithMessage("SystemPhone không đúng định dạng!");

            RuleFor(x => x.SystemEmail).NotEmpty()
                .WithMessage("SystemEmail không thể để trống!")
                .Matches(RegexConstant.Email).WithMessage("SystemEmail không đúng định dạng!");

            RuleFor(x => x.SystemZaloLink).NotEmpty().WithMessage("SystemZaloLink không thể để trống!");

            RuleFor(x => x.SystemYoutubeLink).NotEmpty().WithMessage("SystemYoutubeLink không thể để trống!");

            RuleFor(x => x.SystemFacebookLink).NotEmpty().WithMessage("TSystemFacebookLinkên không thể để trống!");
        }
    }
}