using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Application.Common.Responses;
using Domain.Entities;

namespace Application.News.Queries;

public class GetNewsQuery : IRequest<DataResponse<NewsEntity>>
{
    public int Id { get; }

    public GetNewsQuery(int id)
    {
        Id = id;
    }

    public class Handler : IRequestHandler<GetNewsQuery, DataResponse<NewsEntity>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResponse<NewsEntity>> Handle(GetNewsQuery request, CancellationToken cancellationToken)
        {
            var news = await _context.News
                .Include(x => x.Image)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            if (news == null) { 
                return DataResponse<NewsEntity>.Error("Không tìm thấy tin này!");
            }

            return DataResponse<NewsEntity>.Success(news);

        }
    }
}