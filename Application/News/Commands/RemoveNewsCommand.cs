using Application.Common.Interfaces;
using MediatR;
using Application.Common.Responses;
using Domain.Constants;

namespace Application.News.Commands;

public class RemoveNewsCommand : IRequest<DataResponse<bool>>
{
    public int Id { get; }
    public RemoveNewsCommand(int id)
    {
        Id = id;
    }

    public class Handler : IRequestHandler<RemoveNewsCommand, DataResponse<bool>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResponse<bool>> Handle(RemoveNewsCommand request, CancellationToken cancellationToken)
        {
            var id = request.Id;
            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return DataResponse<bool>.Error("Không tìm thấy bài viết muốn xóa!");
            }

            news.Status = StatusConstant.Removed;

            await _context.SaveChangesAsync(cancellationToken);

            return DataResponse<bool>.Success(true);
        }

    }
}