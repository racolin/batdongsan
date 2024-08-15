using Application.Common.Interfaces;
using MediatR;
using Application.Common.Responses;
using Domain.Constants;

namespace Application.Contacts.Commands;

public class RemoveContactCommand : IRequest<DataResponse<bool>>
{
    public int Id { get; }
    public RemoveContactCommand(int id)
    {
        Id = id;
    }

    public class Handler : IRequestHandler<RemoveContactCommand, DataResponse<bool>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResponse<bool>> Handle(RemoveContactCommand request, CancellationToken cancellationToken)
        {
            var id = request.Id;
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return DataResponse<bool>.Error("Không tìm thấy liên hệ muốn xóa!");
            }

            contact.State = ContactStateConstant.Removed;

            await _context.SaveChangesAsync(cancellationToken);

            return DataResponse<bool>.Success(true);
        }

    }
}