using Application.Common.Mappings;
using Domain.Constants;
using Domain.Entities;
using FluentValidation;

namespace Application.Common.Requests
{
    public class SaveContactRequest : IMapFrom<ContactEntity>
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? State { get; set; }
    }
    public class SaveContactValidator : AbstractValidator<SaveContactRequest>
    {
        public SaveContactValidator()
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

            RuleFor(x => x.State).Must((x, state) =>
            {
                return state == null || ContactStateConstant.GetAllProperties().Contains(state);
            }).WithMessage("Tình trạng này không tồn tại!");
        }
    }
}