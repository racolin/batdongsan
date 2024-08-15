using Application.Common.Interfaces;
using MediatR;
using Application.Common.Responses;
using Application.Common.Requests;
using Domain.Constants;

namespace Application.Contacts.Commands;

public class UpdateStateContactCommand : IRequest<DataResponse<bool>>
{
    public UpdateStateRequest Request { get; }
    public UpdateStateContactCommand(UpdateStateRequest request)
    {
        Request = request;
    }

    public class Handler : IRequestHandler<UpdateStateContactCommand, DataResponse<bool>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResponse<bool>> Handle(UpdateStateContactCommand request, CancellationToken cancellationToken)
        {
            var id = request.Request.Id;
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null) {
                return DataResponse<bool>.Error("Không tìm thấy liên hệ muốn cập nhật!");
            }

            if (!ContactStateConstant.GetAllProperties().Contains(request.Request.State))
            {
                return DataResponse<bool>.Error("Trạng thái gửi lên không chính xác!");
            }

            contact.State = request.Request.State;

            await _context.SaveChangesAsync(cancellationToken);

            return DataResponse<bool>.Success(true);
        }

    }
}