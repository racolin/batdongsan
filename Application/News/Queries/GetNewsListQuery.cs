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
            var status = request.Request.Status;

            var highLight = new List<bool>();
            var lst = request.Request.ValueFilter1?.Split(",");
            if (lst == null || lst.Length == 0) {
                highLight.AddRange([false, true]);
            } else
            {
                foreach (var h in request.Request.ValueFilter1?.Split(",")!)
                {
                    if (h.ToLower().Equals("true"))
                    {
                        highLight.Add(true);
                    }
                    if (h.ToLower().Equals("false"))
                    {
                        highLight.Add(false);
                    }
                }
            }

            var type = request.Request.ValuesFilter1;

            var value = request.Request.Value;
            // Lấy số lượng tin được lọc theo yêu cầu
            var count = await _context.News.AsNoTracking()
                .Where(x =>
                    (value == null ? true : x.Name.Contains(value))
                    && (!x.CreatedDate.HasValue
                        || ((!request.Request.StartDate.HasValue || request.Request.StartDate.Value <= x.CreatedDate.Value)
                            && (!request.Request.EndDate.HasValue || request.Request.EndDate.Value >= x.CreatedDate.Value)))
                    && type.Contains(x.Type)
                    && highLight.Contains(x.IsHighlight)
                    && status.Contains(x.Status)
                ).CountAsync(cancellationToken);

            // Order
            Expression<Func<NewsEntity, Object?>> order = null;
            var isAsc = request.Request.IsAsc ?? true;
            switch (request.Request.Order)
            {
                case "date":
                    order = item => item.CreatedDate;
                    break;
                case "order":
                default:
                    order = item => item.Order;
                    break;
            }

            // Lấy danh sách theo phân trang
            if (request.Request.Start >= count)
            {
                request.Request.Start = 0;
            }
            List<NewsEntity> images;
            if (isAsc)
            {
                images = await _context.News.AsNoTracking()
                    .Include(x => x.Image)
                    .Where(x =>
                        (value == null ? true : x.Name.Contains(value))
                        && (!x.CreatedDate.HasValue
                            || ((!request.Request.StartDate.HasValue || request.Request.StartDate.Value <= x.CreatedDate.Value)
                                && (!request.Request.EndDate.HasValue || request.Request.EndDate.Value >= x.CreatedDate.Value)))
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
                images = await _context.News.AsNoTracking()
                .Include(x => x.Image)
                .Where(x =>
                    (value == null ? true : x.Name.Contains(value))
                    && (!x.CreatedDate.HasValue
                        || ((!request.Request.StartDate.HasValue || request.Request.StartDate.Value <= x.CreatedDate.Value)
                            && (!request.Request.EndDate.HasValue || request.Request.EndDate.Value >= x.CreatedDate.Value)))
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
                List = images,
                PerPage = request.Request.Length,
                Max = max,
                Current = current,
            };

            return DataResponse<PagingResponse<NewsEntity>>.Success(paging);

        }
    }
}