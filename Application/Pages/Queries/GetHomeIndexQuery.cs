using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Application.Common.Responses.Views;
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

            await _context.SaveChangesAsync(cancellationToken);

            var result = await _context.Contents.AsNoTracking()
                .Include(x => x.HomeImage)
                .Include(x => x.BgHomeImage)
                .Select(x => new { x.HomeImage, x.BgHomeImage, x.IntroduceSection, x.NewsMarketSection, x.NewsProjectSection, x.Status })
                .Where(x => x.Status == StatusConstant.Active)
                .FirstOrDefaultAsync(cancellationToken);
            if (result != null)
            {
                _mapper.Map(result!.HomeImage, view.TopImage);
                _mapper.Map(result!.BgHomeImage, view.BackgroundImage);
                view.Introduce = result!.IntroduceSection;
                view.DescriptionNewsProject = result!.NewsMarketSection;
                view.DescriptionNewsMarket = result!.NewsProjectSection;
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