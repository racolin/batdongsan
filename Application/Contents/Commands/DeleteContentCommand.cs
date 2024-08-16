using Application.Common.Interfaces;
using MediatR;
using Application.Common.Responses;
using Domain.Constants;

namespace Application.Contents.Commands;

public class DeleteContentCommand : IRequest<DataResponse<bool>>
{
    public int Id { get; }
    public DeleteContentCommand(int id)
    {
        Id = id;
    }

    public class Handler : IRequestHandler<DeleteContentCommand, DataResponse<bool>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResponse<bool>> Handle(DeleteContentCommand request, CancellationToken cancellationToken)
        {
            var id = request.Id;
            var content = await _context.Contents.FindAsync(id);
            if (content == null)
            {
                return DataResponse<bool>.Error("Không tìm thấy liên hệ muốn xóa!");
            }
            if (content.Status == StatusConstant.Active)
            {
                return DataResponse<bool>.Error("Không thể xóa nội dung web đang hoạt động!");
            }

            _context.Contents.Remove(content);

            await _context.SaveChangesAsync(cancellationToken);

            return DataResponse<bool>.Success(true);
        }

    }
}