using Application.Common.Interfaces;
using Application.Common.Requests;
using Application.Common.Responses;
using Domain.Constants;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.News.Queries;

public class GetNewsListQuery : IRequest<DataResponse<PagingResponse<NewsEntity>>>
{
    public SearchRequest Request { get; }

    public GetNewsListQuery(SearchRequest request)
    {
        Request = request;
    }

    public class Handler : IRequestHandler<GetNewsListQuery, DataResponse<PagingResponse<NewsEntity>>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResponse<PagingResponse<NewsEntity>>> Handle(GetNewsListQuery request, CancellationToken cancellationToken)
        {
            var status = request.Request.Status.Count > 0 ? request.Request.Status : StatusConstant.GetAllProperties();
            var highLight = request.Request.HighLight.Count > 0 ? request.Request.HighLight : new List<bool> { true, false };
            var type = request.Request.Type.Count > 0 ? request.Request.Type : NewsTypeConstant.GetAllProperties();

            var value = request.Request.Value;
            // Lấy số lượng tin được lọc theo yêu cầu
            var count = await _context.News.AsNoTracking()
                .Where(x =>
                    (value == null ? true : x.Name.Contains(value))
                    && (!x.CreatedDate.HasValue
                        || ((!request.Request.StartDateTime.HasValue || request.Request.StartDateTime.Value <= x.CreatedDate.Value)
                            && (!request.Request.EndDateTime.HasValue || request.Request.EndDateTime.Value >= x.CreatedDate.Value)))
                    && type.Contains(x.Type)
                    && highLight.Contains(x.IsHighlight)
                    && status.Contains(x.Status)
                ).CountAsync(cancellationToken);

            // Order
            Expression<Func<NewsEntity, Object?>> order = null;
            var isAsc = request.Request.IsAsc ?? true;
            switch (request.Request.Order)
            {
                case "order":
                    order = item => item.Order;
                    break;
                case "date":
                default:
                    order = item => item.CreatedDate;
                    break;
            }

            if (count < request.Request.Start)
            {
                request.Request.CurrentPage = 0;
            }

            List<NewsEntity> newsList;
            if (isAsc)
            {
                newsList = await _context.News.AsNoTracking()
                    .Include(x => x.Image)
                    .Where(x =>
                        (value == null ? true : x.Name.Contains(value))
                        && (!x.CreatedDate.HasValue
                            || ((!request.Request.StartDateTime.HasValue || request.Request.StartDateTime.Value <= x.CreatedDate.Value)
                                && (!request.Request.EndDateTime.HasValue || request.Request.EndDateTime.Value >= x.CreatedDate.Value)))
                        && type.Contains(x.Type)
                        && highLight.Contains(x.IsHighlight)
                        && status.Contains(x.Status)
                    )
                    .OrderBy(x => x.CreatedDate)
                    .Skip(request.Request.Start)
                    .Take(request.Request.Length)
                    .ToListAsync(cancellationToken);
            }
            else
            {
                newsList = await _context.News.AsNoTracking()
                .Include(x => x.Image)
                .Where(x =>
                    (value == null ? true : x.Name.Contains(value))
                    && (!x.CreatedDate.HasValue
                        || ((!request.Request.StartDateTime.HasValue || request.Request.StartDateTime.Value <= x.CreatedDate.Value)
                            && (!request.Request.EndDateTime.HasValue || request.Request.EndDateTime.Value >= x.CreatedDate.Value)))
                    && type.Contains(x.Type)
                    && highLight.Contains(x.IsHighlight)
                    && status.Contains(x.Status)
                )
                .OrderByDescending(order)
                .Skip(request.Request.Start)
                .Take(request.Request.Length)
                .ToListAsync(cancellationToken);
            }

            var max = count / request.Request.Length + (count % request.Request.Length == 0 ? 0 : 1);
            var current = (request.Request.Start + 1) / request.Request.Length + ((request.Request.Start + 1) % request.Request.Length == 0 ? 0 : 1);

            var paging = new PagingResponse<NewsEntity>
            {
                List = newsList,
                PerPage = request.Request.Length,
                Max = max,
                Current = current,
            };

            return DataResponse<PagingResponse<NewsEntity>>.Success(paging);

        }
    }
}