using FluentValidation;

namespace Application.Common.Requests
{
    public class UpdateStateRequest
    {
        public int Id { get; set; }
        public string State { get; set; } = string.Empty;
    }
    public class UpdateStateValidator : AbstractValidator<UpdateStateRequest>
    {
        public UpdateStateValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id của đối tượng không chính xác!");

            RuleFor(x => x.State).NotEmpty().WithMessage("Tình tình không được trống!");
        }
    }
}
