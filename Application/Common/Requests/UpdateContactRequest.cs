using Application.Common.Mappings;
using Domain.Entities;
using FluentValidation;

namespace Application.Common.Requests
{
    public class UpdateContactRequest : UpdateStateRequest, IMapFrom<ContactEntity>
    {
        public string Note { get; set; } = string.Empty;
    }
    public class UpdateContactValidator : AbstractValidator<UpdateContactRequest>
    {
        public UpdateContactValidator()
        {

            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id của đối tượng không chính xác!");
        }
    }
}