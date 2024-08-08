using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Application.Common.Responses.Views;
using Domain.Enums;
using AutoMapper;
using Domain.Constants;

namespace Application.Pages.Queries;

public class GetHomeIndexQuery : IRequest<HomeIndexView>
{
    public GetHomeIndexQuery() {}

    public class Handler : IRequestHandler<GetHomeIndexQuery, HomeIndexView>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<HomeIndexView> Handle(GetHomeIndexQuery request, CancellationToken cancellationToken)
        {
            var view = new HomeIndexView();

            var section = await _context.Sections.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Position == (int)SectionPositionEnum.IntroduceInHome, cancellationToken);
            if (section != null) {
                view.Content = section!.Content;
            }

            await _context.SaveChangesAsync(cancellationToken);

            var background = await _context.ImagePages.AsNoTracking()
                .Include(x => x.Image)
                .FirstOrDefaultAsync(x => x.Position == (int)ImagePagePositionEnum.BackgroundHomeScreen, cancellationToken);
            if (background != null) {
                _mapper.Map(background!.Image, view.BackgroundImage);
            }

            var image = await _context.ImagePages.AsNoTracking()
                .Include(x => x.Image)
                .FirstOrDefaultAsync(x => x.Position == (int)ImagePagePositionEnum.HomeScreen, cancellationToken);
            if (image != null) {
                _mapper.Map(image!.Image, view.TopImage);
            }

            var news = await _context.News.AsNoTracking()
                .Include(x => x.Image)
                .Where(x => x.Status == StatusConstant.Active && x.IsHighlight)
                .ToListAsync(cancellationToken);
            _mapper.Map(news, view.NewsList);

            var projects = await _context.Projects.AsNoTracking()
                .Include(x => x.Image)
                .Where(x => x.Status == StatusConstant.Active && x.IsHighlight)
                .ToListAsync(cancellationToken);
            _mapper.Map(projects, view.Projects);

            return view;
        }
    }
}