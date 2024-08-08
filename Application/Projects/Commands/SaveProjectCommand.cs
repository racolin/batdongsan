using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;
using Application.Common.Requests;
using Application.Common.Responses;
using Domain.Entities;

namespace Application.Project.Commands;
public class SaveProjectCommand : IRequest<DataResponse<int>>
{
    public SaveProjectRequest Request { get; }

    public SaveProjectCommand(SaveProjectRequest request)
    {
        Request = request;
    }

    public class Handler : IRequestHandler<SaveProjectCommand, DataResponse<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DataResponse<int>> Handle(SaveProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == request.Request.Id, cancellationToken);
            if (project == null)
            {
                project = _mapper.Map<ProjectEntity>(request.Request);
                _context.Projects.Add(project);
            }
            else
            {
                // TODO
                var content = project.Content;
                _mapper.Map(request.Request, project);

                if (!request.Request.IsUpdateContent)
                {
                    project.Content = content;
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            return new DataResponse<int> { Data = project.Id };
        }
    }
}