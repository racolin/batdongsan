using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Application.Common.Responses.Views;
using AutoMapper;
using Domain.Entities;
using Domain.Constants;
using Application.Common.Supports;

namespace Application.Pages.Queries;

public class GetProjectIndexQuery : IRequest<ProjectIndexView>
{
    public GetProjectIndexQuery() {}

    public class Handler : IRequestHandler<GetProjectIndexQuery, ProjectIndexView>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProjectIndexView> Handle(GetProjectIndexQuery request, CancellationToken cancellationToken)
        {
            var view = new ProjectIndexView();

            var result = await _context.Contents.AsNoTracking()
                .Include(x => x.ProjectSlider).ThenInclude(x => x.Image)
                .Select(x => new { x.ProjectSlider, x.Status })
                .Where(x => x.Status == StatusConstant.Active)
                .FirstOrDefaultAsync(cancellationToken);
            if (result != null)
            {
                var slider = result.ProjectSlider.OrderBy(x => x.Order);

                foreach (var item in slider)
                {
                    var img = _mapper.Map<ImageEntity, ImageView>(item.Image);
                    if (img != null)
                    {
                        img.Link = item.Link;
                        view.Slides.Add(img);
                    }
                }
            }

            var groups = await _context.Projects.AsNoTracking()
                .Include(x => x.Image)
                .Where(x => x.Status == StatusConstant.Active)
                .GroupBy(x => new { x.Type, x.State })
                .Select(x => new
                {
                    Type = x.Key.Type,
                    State = x.Key.State,
                    Projects = x.ToList(),
                })
                .OrderBy(x => x.Type)
                .ThenByDescending(m => m.State)
                .ToListAsync(cancellationToken);

            foreach (var group in groups)
            {
                if (group.Type == ProjectTypeConstant.Apartment)
                {
                    view.Apartment.Add(new ProjectGroup
                    {
                        State = group.State,
                        StateName = NameSupport.GetProjectStateName(group.State) ?? "",
                        Projects = _mapper.Map<List<ProjectView>>(group.Projects),
                    });
                }
                if (group.Type == ProjectTypeConstant.Ground)
                {
                    view.Ground.Add(new ProjectGroup
                    {
                        State = group.State,
                        StateName = NameSupport.GetProjectStateName(group.State) ?? "",
                        Projects = _mapper.Map<List<ProjectView>>(group.Projects),
                    });
                }
                if (group.Type == ProjectTypeConstant.ResortRealEstate)
                {
                    view.ResortRealEstate.Add(new ProjectGroup
                    {
                        State = group.State,
                        StateName = NameSupport.GetProjectStateName(group.State) ?? "",
                        Projects = _mapper.Map<List<ProjectView>>(group.Projects),
                    });
                }
                if (group.Type == ProjectTypeConstant.Villa)
                {
                    view.Villa.Add(new ProjectGroup
                    {
                        State = group.State,
                        StateName = NameSupport.GetProjectStateName(group.State) ?? "",
                        Projects = _mapper.Map<List<ProjectView>>(group.Projects),
                    });
                }
            }

            return view;
        }
    }
}