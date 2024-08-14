using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Application.Common.Responses;
using Domain.Entities;
using Application.Common.Requests;
using Domain.Constants;
using System.Linq.Expressions;

namespace Application.Contents.Queries;

public class GetContentsQuery : IRequest<DataResponse<PagingResponse<ContentEntity>>>
{
    public SearchRequest Request { get; }

    public GetContentsQuery(SearchRequest request)
    {
        Request = request;
    }

    public class Handler : IRequestHandler<GetContentsQuery, DataResponse<PagingResponse<ContentEntity>>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResponse<PagingResponse<ContentEntity>>> Handle(GetContentsQuery request, CancellationToken cancellationToken)
        {
            var status = request.Request.Status.Count > 0 ? request.Request.Status : StatusConstant.GetAllProperties();

            var value = request.Request.Value;
            // Lấy số lượng tin được lọc theo yêu cầu
            var count = await _context.Contents.AsNoTracking()
                .Where(x =>
                    (value == null ? true : x.Name.Contains(value))
                    && (!x.CreatedDate.HasValue
                        || ((!request.Request.StartDateTime.HasValue || request.Request.StartDateTime.Value <= x.CreatedDate.Value)
                            && (!request.Request.EndDateTime.HasValue || request.Request.EndDateTime.Value >= x.CreatedDate.Value)))
                    && status.Contains(x.Status)
                ).CountAsync(cancellationToken);

            // Order
            Expression<Func<ContentEntity, Object?>> order = null;
            var isAsc = request.Request.IsAsc ?? true;
            switch (request.Request.Order)
            {
                case "date":
                default:
                    order = item => item.CreatedDate;
                    break;
            }

            if (count < request.Request.Start)
            {
                request.Request.CurrentPage = 0;
            }

            List<ContentEntity> contents;
            if (isAsc)
            {
                contents = await _context.Contents.AsNoTracking()
                    .Where(x =>
                        (value == null ? true : x.Name.Contains(value))
                        && (!x.CreatedDate.HasValue
                            || ((!request.Request.StartDateTime.HasValue || request.Request.StartDateTime.Value <= x.CreatedDate.Value)
                                && (!request.Request.EndDateTime.HasValue || request.Request.EndDateTime.Value >= x.CreatedDate.Value)))
                        && status.Contains(x.Status)
                    )
                    .OrderBy(x => x.CreatedDate)
                    .Skip(request.Request.Start)
                    .Take(request.Request.Length)
                    .ToListAsync(cancellationToken);
            }
            else
            {
                contents = await _context.Contents.AsNoTracking()
                .Where(x =>
                    (value == null ? true : x.Name.Contains(value))
                    && (!x.CreatedDate.HasValue
                        || ((!request.Request.StartDateTime.HasValue || request.Request.StartDateTime.Value <= x.CreatedDate.Value)
                            && (!request.Request.EndDateTime.HasValue || request.Request.EndDateTime.Value >= x.CreatedDate.Value)))
                    && status.Contains(x.Status)
                )
                .OrderByDescending(order)
                .Skip(request.Request.Start)
                .Take(request.Request.Length)
                .ToListAsync(cancellationToken);
            }

            var max = count / request.Request.Length + (count % request.Request.Length == 0 ? 0 : 1);
            var current = (request.Request.Start + 1) / request.Request.Length + ((request.Request.Start + 1) % request.Request.Length == 0 ? 0 : 1);

            var paging = new PagingResponse<ContentEntity>
            {
                List = contents,
                PerPage = request.Request.Length,
                Max = max,
                Current = current,
            };

            return DataResponse<PagingResponse<ContentEntity>>.Success(paging);

        }
    }
}