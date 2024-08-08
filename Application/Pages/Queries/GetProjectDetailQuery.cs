using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Application.Common.Responses.Views;
using AutoMapper;
using Domain.Constants;

namespace Application.Pages.Queries;

public class GetNewsDetailQuery : IRequest<NewsDetailView?>
{
    public string? Ud { get; }
    public GetNewsDetailQuery(string? ud) {
        Ud = ud;
    }

    public class Handler : IRequestHandler<GetNewsDetailQuery, NewsDetailView?>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<NewsDetailView?> Handle(GetNewsDetailQuery request, CancellationToken cancellationToken)
        {
            var view = new NewsDetailView();

            var news = await _context.News.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Status == StatusConstant.Active && x.Ud.Equals(request.Ud));
            if (news == null)
            {
                return null;
            }
            _mapper.Map(news!, view.News);

            var relatedNews = await _context.News.AsNoTracking()
                .Where(x => 
                    x.Status == StatusConstant.Active 
                    && news.Order != null 
                    && x.Order != null 
                    && (x.Order == news.Order - 1 || x.Order == news.Order + 1)
                )
                .ToListAsync(cancellationToken);
            if (relatedNews != null)
            {
                view.HintType = 0;
                foreach (var item in relatedNews) {
                    if (item.Order == news.Order - 1)
                    {
                        view.HintLeftName = item.Name;
                        view.HintLeftUd = item.Ud;
                        view.HintType += 1;
                    }
                    else
                    {
                        view.HintRightName = item.Name;
                        view.HintRightUd = item.Ud;
                        view.HintType += 2;
                    }
                }
            }

            return view;
        }
    }
}