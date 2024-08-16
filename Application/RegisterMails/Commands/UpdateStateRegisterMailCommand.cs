using Application.Common.Interfaces;
using MediatR;
using Application.Common.Responses;
using Application.Common.Requests;
using Domain.Constants;

namespace Application.RegisterMails.Commands;

public class UpdateStateRegisterMailCommand : IRequest<DataResponse<bool>>
{
    public UpdateStateRequest Request { get; }
    public UpdateStateRegisterMailCommand(UpdateStateRequest request)
    {
        Request = request;
    }

    public class Handler : IRequestHandler<UpdateStateRegisterMailCommand, DataResponse<bool>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResponse<bool>> Handle(UpdateStateRegisterMailCommand request, CancellationToken cancellationToken)
        {
            var id = request.Request.Id;
            var contact = await _context.RegisterMails.FindAsync(id);
            if (contact == null) {
                return DataResponse<bool>.Error("Không tìm thấy email muốn cập nhật!");
            }

            if (!RegisterMailStateConstant.GetAllProperties().Contains(request.Request.State))
            {
                return DataResponse<bool>.Error("Tình trạng gửi lên không chính xác!");
            }

            contact.State = request.Request.State;

            await _context.SaveChangesAsync(cancellationToken);

            return DataResponse<bool>.Success(true);
        }

    }
}