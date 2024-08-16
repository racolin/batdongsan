using Application.Common.Interfaces;
using MediatR;
using Application.Common.Responses;
using Application.Common.Requests;
using Domain.Constants;

namespace Application.News.Commands;

public class UpdateStatusNewsCommand : IRequest<DataResponse<bool>>
{
    public UpdateStatusRequest Request { get; }
    public UpdateStatusNewsCommand(UpdateStatusRequest request)
    {
        Request = request;
    }

    public class Handler : IRequestHandler<UpdateStatusNewsCommand, DataResponse<bool>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResponse<bool>> Handle(UpdateStatusNewsCommand request, CancellationToken cancellationToken)
        {
            var id = request.Request.Id;
            var news = await _context.News.FindAsync(id);
            if (news == null) {
                return DataResponse<bool>.Error("Không tìm thấy bài viết muốn cập nhật!");
            }

            if (!StatusConstant.GetAllProperties().Contains(request.Request.Status))
            {
                return DataResponse<bool>.Error("Tình trạng gửi lên không chính xác!");
            }

            news.Status = request.Request.Status;

            await _context.SaveChangesAsync(cancellationToken);

            return DataResponse<bool>.Success(true);
        }

    }
}