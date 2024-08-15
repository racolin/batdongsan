using Domain.Enums;
using FluentValidation;

namespace Application.Common.Requests
{
    public class SaveProfileRequest
    {
        public string Name { get; set; } = string.Empty;
        public int Gender { get; set; } = (int)GenderEnum.Male;
        public string? DateOfBirth { get; set; }
    }
    public class SaveProfileValidator : AbstractValidator<SaveProfileRequest>
    {
        public SaveProfileValidator()
        {

            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên không thể để trống!");

            RuleFor(x => x.Gender).Must((x, gender) => {
                return (new List<int> { (int)GenderEnum.Female, (int)GenderEnum.Male, (int)GenderEnum.Others }).Contains(gender);
            }).WithMessage("Giới tính không đúng định dạng!");
        }
    }
}