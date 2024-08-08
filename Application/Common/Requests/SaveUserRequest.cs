using Domain.Constants;
using Domain.Enums;
using FluentValidation;

namespace Application.Common.Requests
{
    public class SaveUserRequest
    {
        public int? Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int Gender { get; set; } = (int)GenderEnum.Male;
        public string? DateOfBirth { get; set; }
    }
    public class SaveUserValidator : AbstractValidator<SaveUserRequest>
    {
        public SaveUserValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Tên đăng nhập không thể để trống!");

            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên không thể để trống!");

            RuleFor(x => x.Gender).Must((x, gender) => {
                return (new List<int> { (int)GenderEnum.Female, (int)GenderEnum.Male, (int)GenderEnum.Others }).Contains(gender);
            }).WithMessage("Giới tính không đúng định dạng!");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email không thể để để trống!")
                .Matches(RegexConstant.Email).WithMessage("Email không đúng định dạng!");
        }
    }
}