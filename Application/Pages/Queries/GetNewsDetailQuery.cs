using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Application.Common.Responses.Views;
using AutoMapper;
using Domain.Constants;

namespace Application.Pages.Queries;

public class GetProjectDetailQuery : IRequest<ProjectView?>
{
    public string? Ud { get; }
    public GetProjectDetailQuery(string? ud) {
        Ud = ud;
    }

    public class Handler : IRequestHandler<GetProjectDetailQuery, ProjectView?>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProjectView?> Handle(GetProjectDetailQuery request, CancellationToken cancellationToken)
        {
            var view = new ProjectView();

            var project = await _context.Projects.AsNoTracking()
                .Include(x => x.Image)
                .FirstOrDefaultAsync(x => x.Status == StatusConstant.Active && x.Ud.Equals(request.Ud));
            if (project == null)
            {
                return null;
            }
            _mapper.Map(project!, view);

            return view;
        }
    }
}