using Application.Common.Interfaces;
using MediatR;
using Application.Common.Responses;
using Domain.Constants;

namespace Application.RegisterMails.Commands;

public class RemoveRegisterMailCommand : IRequest<DataResponse<bool>>
{
    public int Id { get; }
    public RemoveRegisterMailCommand(int id)
    {
        Id = id;
    }

    public class Handler : IRequestHandler<RemoveRegisterMailCommand, DataResponse<bool>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResponse<bool>> Handle(RemoveRegisterMailCommand request, CancellationToken cancellationToken)
        {
            var id = request.Id;
            var registerMail = await _context.RegisterMails.FindAsync(id);
            if (registerMail == null)
            {
                return DataResponse<bool>.Error("Không tìm thấy email muốn xóa!");
            }

            registerMail.State = RegisterMailStateConstant.Removed;

            await _context.SaveChangesAsync(cancellationToken);

            return DataResponse<bool>.Success(true);
        }

    }
}