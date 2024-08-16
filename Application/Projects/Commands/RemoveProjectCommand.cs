using Application.Common.Interfaces;
using MediatR;
using Application.Common.Responses;
using Domain.Constants;

namespace Application.Projects.Commands;

public class RemoveProjectCommand : IRequest<DataResponse<bool>>
{
    public int Id { get; }
    public RemoveProjectCommand(int id)
    {
        Id = id;
    }

    public class Handler : IRequestHandler<RemoveProjectCommand, DataResponse<bool>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResponse<bool>> Handle(RemoveProjectCommand request, CancellationToken cancellationToken)
        {
            var id = request.Id;
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return DataResponse<bool>.Error("Không tìm thấy dự án muốn xóa!");
            }

            project.Status = StatusConstant.Removed;

            await _context.SaveChangesAsync(cancellationToken);

            return DataResponse<bool>.Success(true);
        }

    }
}