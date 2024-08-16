using Application.Common.Interfaces;
using MediatR;
using Application.Common.Responses;

namespace Application.Projects.Commands;

public class DeleteProjectCommand : IRequest<DataResponse<bool>>
{
    public int Id { get; }
    public DeleteProjectCommand(int id)
    {
        Id = id;
    }

    public class Handler : IRequestHandler<DeleteProjectCommand, DataResponse<bool>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResponse<bool>> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var id = request.Id;
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return DataResponse<bool>.Error("Không tìm thấy bài viết muốn xóa!");
            }

            _context.Projects.Remove(project);

            await _context.SaveChangesAsync(cancellationToken);

            return DataResponse<bool>.Success(true);
        }

    }
}