using Application.Common.Mappings;
using Domain.Constants;
using Domain.Entities;
using FluentValidation;

namespace Application.Common.Requests
{
    public class SaveRegisterMailRequest : IMapFrom<RegisterMailEntity>
    {
        public int? Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? State { get; set; }
    }
    public class SaveRegisterMailValidator : AbstractValidator<SaveRegisterMailRequest>
    {
        public SaveRegisterMailValidator()
        {

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email không thể để để trống!")
                .Matches(RegexConstant.Email).WithMessage("Email không đúng định dạng!");

            RuleFor(x => x.State).Must((x, state) =>
            {
                return state == null || RegisterMailStateConstant.GetAllProperties().Contains(state);
            }).WithMessage("Tình trạng này không tồn tại!");
        }
    }
}