using Application.Common.Interfaces;
using MediatR;
using Application.Common.Responses;
using Domain.Constants;

namespace Application.Contents.Commands;

public class RemoveContentCommand : IRequest<DataResponse<bool>>
{
    public int Id { get; }
    public RemoveContentCommand(int id)
    {
        Id = id;
    }

    public class Handler : IRequestHandler<RemoveContentCommand, DataResponse<bool>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResponse<bool>> Handle(RemoveContentCommand request, CancellationToken cancellationToken)
        {
            var id = request.Id;
            var content = await _context.Contents.FindAsync(id);
            if (content == null)
            {
                return DataResponse<bool>.Error("Không tìm thấy nội dung web muốn xóa!");
            }
            if (content.Status == StatusConstant.Active)
            {
                return DataResponse<bool>.Error("Không thể xóa nội dung web đang hoạt động!");
            }

            content.Status = StatusConstant.Removed;

            await _context.SaveChangesAsync(cancellationToken);

            return DataResponse<bool>.Success(true);
        }

    }
}