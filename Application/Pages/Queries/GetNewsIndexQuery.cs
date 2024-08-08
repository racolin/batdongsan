using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Application.Common.Responses.Views;
using Domain.Enums;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;

namespace Application.Pages.Queries;

public class GetNewsIndexQuery : IRequest<NewsIndexView>
{
    public int Current {  get; }
    public int PerPage { get; } = 9;
    public string? Type { get; }
    public GetNewsIndexQuery(int page, string? type) {
        Current = page > 0 ? page : 1;    
        Type = type;
    }
    public GetNewsIndexQuery(int page)
    {
        Current = page > 0 ? page : 1;
    }

    public class Handler : IRequestHandler<GetNewsIndexQuery, NewsIndexView>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<NewsIndexView> Handle(GetNewsIndexQuery request, CancellationToken cancellationToken)
        {
            var view = new NewsIndexView();

            var image = await _context.ImagePages.AsNoTracking()
                .Include(x => x.Image)
                .FirstOrDefaultAsync(x => x.Position == (int)ImagePagePositionEnum.NewsScreen, cancellationToken);
            if (image != null)
            {
                _mapper.Map(image!.Image, view.TopImage);
            }

            var count = await _context.News
                .Where(x => x.Status == StatusConstant.Active && (null == request.Type || x.Type == request.Type))
                .CountAsync(x => x.Status == StatusConstant.Active);
            view.Paging.Max = count / request.PerPage + (count % request.PerPage == 0 ? 0 : 1);
            if (count > (request.Current - 1) * request.PerPage) {
                view.Paging.Current = request.Current;
            } else
            {
                view.Paging.Current = 1;
            }

            var news = await _context.News.AsNoTracking()
                .Select(x => new NewsEntity
                {
                    Id = x.Id,
                    Ud = x.Ud,
                    Name = x.Name,
                    Type = x.Type,
                    Description = x.Description,
                    Status = x.Status,
                    Order = x.Order,
                    Image = x.Image,
                    CreatedDate = x.CreatedDate,
                })
                .Where(x => x.Status == StatusConstant.Active && (null == request.Type || x.Type == request.Type))
                .OrderBy(x => x.Order)
                .Skip((view.Paging.Current - 1) * request.PerPage)
                .Take(request.PerPage)
                .ToListAsync(cancellationToken);
            _mapper.Map(news, view.Paging.NewsList);

            return view;
        }
    }
}