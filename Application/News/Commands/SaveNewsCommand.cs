using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;
using Application.Common.Requests;
using Application.Common.Responses;
using Domain.Entities;

namespace Application.News.Commands;
public class SaveNewsCommand : IRequest<DataResponse<int>>
{
    public SaveNewsRequest Request { get; }

    public SaveNewsCommand(SaveNewsRequest request)
    {
        Request = request;
    }

    public class Handler : IRequestHandler<SaveNewsCommand, DataResponse<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DataResponse<int>> Handle(SaveNewsCommand request, CancellationToken cancellationToken)
        {
            DateTime? publishDate = null;
            if (request.Request.PubDate != null)
            {
                try
                {
                    publishDate = DateTime.ParseExact(request.Request.PubDate, "dd-MM-yyyy", null);
                }
                catch (Exception ex)
                {

                }
            };
            var news = await _context.News.FirstOrDefaultAsync(x => x.Id == request.Request.Id, cancellationToken);
            if (news == null)
            {
                news = _mapper.Map<NewsEntity>(request.Request);
                _context.News.Add(news);
            }
            else
            {
                // TODO
                var content = news.Content;
                _mapper.Map(request.Request, news);

                if (!request.Request.IsUpdateContent)
                {
                    news.Content = content;
                }
            }

            news.PublishDate = publishDate;

            await _context.SaveChangesAsync(cancellationToken);

            return new DataResponse<int> { Data = news.Id };
        }
    }
}