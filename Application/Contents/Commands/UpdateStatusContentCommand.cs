using Application.Common.Interfaces;
using MediatR;
using Application.Common.Responses;
using Application.Common.Requests;
using Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace Application.Contents.Commands;

public class UpdateStatusContentCommand : IRequest<DataResponse<bool>>
{
    public UpdateStatusRequest Request { get; }
    public UpdateStatusContentCommand(UpdateStatusRequest request)
    {
        Request = request;
    }

    public class Handler : IRequestHandler<UpdateStatusContentCommand, DataResponse<bool>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResponse<bool>> Handle(UpdateStatusContentCommand request, CancellationToken cancellationToken)
        {
            var id = request.Request.Id;
            var content = await _context.Contents.FindAsync(id);
            if (content == null) {
                return DataResponse<bool>.Error("Không tìm thấy nội dung web muốn cập nhật!");
            }
            if (content.Status == StatusConstant.Active)
            {
                return DataResponse<bool>.Error("Không thể cập nhật trạng thái khi đang kích hoạt. Hãy kích hoạt một nội dung web khác trước!");
            }

            if (!StatusConstant.GetAllProperties().Contains(request.Request.Status))
            {
                return DataResponse<bool>.Error("Trạng thái gửi lên không chính xác!");
            }

            content.Status = request.Request.Status;

            if (content.Status == StatusConstant.Active)
            {
                var items = await _context.Contents.Where(x => x.Status == StatusConstant.Active).ToListAsync();
                foreach (var item in items)
                {
                    item.Status = StatusConstant.Draft;
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            return DataResponse<bool>.Success(true);
        }

    }
}