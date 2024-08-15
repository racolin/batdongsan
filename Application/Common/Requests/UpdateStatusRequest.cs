using Domain.Constants;
using FluentValidation;

namespace Application.Common.Requests
{
    public class UpdateStatusRequest
    {
        public int Id { get; set; }
        public string Status { get; set; } = StatusConstant.Draft;
    }
    public class UpdateStatusValidator : AbstractValidator<UpdateStatusRequest>
    {
        public UpdateStatusValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id của đối tượng không chính xác!");

            RuleFor(x => x.Status).Must((x, status) =>
            {
                return new List<string> { 
                    StatusConstant.Draft,
                    StatusConstant.Active,
                    StatusConstant.InActive,
                    StatusConstant.Removed,
                }
                .Contains(status);
            }).WithMessage("Trạng thái không chính xác, hãy thử lại");
        }
    }
}
