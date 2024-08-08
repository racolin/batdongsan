using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Application.Common.Responses;
using Domain.Entities;

namespace Application.Projects.Queries;

public class GetProjectQuery : IRequest<DataResponse<ProjectEntity?>>
{
    public int Id { get; }

    public GetProjectQuery(int id)
    {
        Id = id;
    }

    public class Handler : IRequestHandler<GetProjectQuery, DataResponse<ProjectEntity?>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResponse<ProjectEntity?>> Handle(GetProjectQuery request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects
                .Include(x => x.Image)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            return DataResponse<ProjectEntity?>.Success(project);

        }
    }
}