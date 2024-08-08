using Application.Common.Interfaces;
using Application.Common.Requests;
using Application.Common.Responses;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.Projects.Queries;

public class GetProjectsQuery : IRequest<DataResponse<PagingResponse<ProjectEntity>>>
{
    public SearchRequest Request { get; }

    public GetProjectsQuery(SearchRequest request)
    {
        Request = request;
    }

    public class Handler : IRequestHandler<GetProjectsQuery, DataResponse<PagingResponse<ProjectEntity>>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResponse<PagingResponse<ProjectEntity>>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
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
            var address = request.Request.ValueFilter2;
            var type = request.Request.ValuesFilter1;
            var state = request.Request.ValuesFilter2;

            var value = request.Request.Value;
            // Lấy số lượng dự án được lọc theo yêu cầu
            var count = await _context.Projects.AsNoTracking()
                .Where(x =>
                    (value == null ? true : x.Name.Contains(value))
                    &&(address == null ? true : x.Address.Contains(address))
                    && (!x.CreatedDate.HasValue
                        || ((!request.Request.StartDate.HasValue || request.Request.StartDate.Value <= x.CreatedDate.Value)
                            && (!request.Request.EndDate.HasValue || request.Request.EndDate.Value >= x.CreatedDate.Value)))
                    && state.Contains(x.State)
                    && type.Contains(x.Type)
                    && highLight.Contains(x.IsHighlight)
                    && status.Contains(x.Status)
                ).CountAsync(cancellationToken);

            // Order
            Expression<Func<ProjectEntity, Object?>> order = null;
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
            List<ProjectEntity> images;
            if (isAsc)
            {
                images = await _context.Projects.AsNoTracking()
                    .Include(x => x.Image)
                    .Where(x =>
                        (value == null ? true : x.Name.Contains(value))
                        && (address == null ? true : x.Address.Contains(address))
                        && (!x.CreatedDate.HasValue
                            || ((!request.Request.StartDate.HasValue || request.Request.StartDate.Value <= x.CreatedDate.Value)
                                && (!request.Request.EndDate.HasValue || request.Request.EndDate.Value >= x.CreatedDate.Value)))
                        && state.Contains(x.State)
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
                images = await _context.Projects.AsNoTracking()
                .Include(x => x.Image)
                .Where(x =>
                    (value == null ? true : x.Name.Contains(value))
                    && (address == null ? true : x.Address.Contains(address))
                    && (!x.CreatedDate.HasValue
                        || ((!request.Request.StartDate.HasValue || request.Request.StartDate.Value <= x.CreatedDate.Value)
                            && (!request.Request.EndDate.HasValue || request.Request.EndDate.Value >= x.CreatedDate.Value)))
                    && state.Contains(x.State)
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

            var paging = new PagingResponse<ProjectEntity>
            {
                List = images,
                PerPage = request.Request.Length,
                Max = max,
                Current = current,
            };

            return DataResponse<PagingResponse<ProjectEntity>>.Success(paging);

        }
    }
}