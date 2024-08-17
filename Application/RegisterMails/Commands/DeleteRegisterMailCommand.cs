using Application.Common.Interfaces;
using MediatR;
using Application.Common.Responses;
using Domain.Constants;

namespace Application.RegisterMails.Commands;

public class DeleteRegisterMailCommand : IRequest<DataResponse<bool>>
{
    public int Id { get; }
    public DeleteRegisterMailCommand(int id)
    {
        Id = id;
    }

    public class Handler : IRequestHandler<DeleteRegisterMailCommand, DataResponse<bool>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResponse<bool>> Handle(DeleteRegisterMailCommand request, CancellationToken cancellationToken)
        {
            var id = request.Id;
            var registerMail = await _context.RegisterMails.FindAsync(id);
            if (registerMail == null)
            {
                return DataResponse<bool>.Error("Không tìm thấy email muốn xóa!");
            }

            _context.RegisterMails.Remove(registerMail);

            await _context.SaveChangesAsync(cancellationToken);

            return DataResponse<bool>.Success(true);
        }

    }
}