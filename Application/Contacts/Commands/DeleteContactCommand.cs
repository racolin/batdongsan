using Application.Common.Interfaces;
using MediatR;
using Application.Common.Responses;

namespace Application.Contacts.Commands;

public class DeleteContactCommand : IRequest<DataResponse<bool>>
{
    public int Id { get; }
    public DeleteContactCommand(int id)
    {
        Id = id;
    }

    public class Handler : IRequestHandler<DeleteContactCommand, DataResponse<bool>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResponse<bool>> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
        {
            var id = request.Id;
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return DataResponse<bool>.Error("Không tìm thấy liên hệ muốn xóa!");
            }

            _context.Contacts.Remove(contact);

            await _context.SaveChangesAsync(cancellationToken);

            return DataResponse<bool>.Success(true);
        }

    }
}