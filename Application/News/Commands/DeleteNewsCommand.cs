using Application.Common.Interfaces;
using MediatR;
using Application.Common.Responses;

namespace Application.News.Commands;

public class DeleteNewsCommand : IRequest<DataResponse<bool>>
{
    public int Id { get; }
    public DeleteNewsCommand(int id)
    {
        Id = id;
    }

    public class Handler : IRequestHandler<DeleteNewsCommand, DataResponse<bool>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResponse<bool>> Handle(DeleteNewsCommand request, CancellationToken cancellationToken)
        {
            var id = request.Id;
            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return DataResponse<bool>.Error("Không tìm thấy bài viết muốn xóa!");
            }

            _context.News.Remove(news);

            await _context.SaveChangesAsync(cancellationToken);

            return DataResponse<bool>.Success(true);
        }

    }
}