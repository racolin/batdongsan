using Application.Common.Interfaces;
using Application.Common.Requests;
using Application.Common.Responses;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.Images.Queries;

public class GetImagesQuery : IRequest<DataResponse<PagingResponse<ImageEntity>>>
{
    public SearchRequest Request { get; }

    public GetImagesQuery(SearchRequest request)
    {
        Request = request;
    }

    public class Handler : IRequestHandler<GetImagesQuery, DataResponse<PagingResponse<ImageEntity>>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResponse<PagingResponse<ImageEntity>>> Handle(GetImagesQuery request, CancellationToken cancellationToken)
        {
            var value = request.Request.Value;
            // Lấy số lượng ảnh được lọc theo yêu cầu
            var count = await _context.Images.AsNoTracking()
                .Where(x =>
                    (value == null ? true : x.Title.Contains(value))
                    && (!x.CreatedDate.HasValue
                        || ((!request.Request.StartDateTime.HasValue || request.Request.StartDateTime.Value <= x.CreatedDate.Value)
                            && (!request.Request.EndDateTime.HasValue || request.Request.EndDateTime.Value >= x.CreatedDate.Value)))
                ).CountAsync(cancellationToken);

            // Order
            Expression<Func<ImageEntity, Object?>> order = null;
            var isAsc = request.Request.IsAsc ?? true;
            switch (request.Request.Order)
            {
                case "date":
                default:
                    order = item => item.CreatedDate;
                    break;
            }

            if (count < request.Request.Start) { 
                request.Request.CurrentPage = 0;
            }

            List<ImageEntity> images;
            if (isAsc) {
                images = await _context.Images.AsNoTracking()
                    .Where(x =>
                        (value == null ? true : x.Title.Contains(value))
                        && (!x.CreatedDate.HasValue
                            || ((!request.Request.StartDateTime.HasValue || request.Request.StartDateTime.Value <= x.CreatedDate.Value)
                                && (!request.Request.EndDateTime.HasValue || request.Request.EndDateTime.Value >= x.CreatedDate.Value)))
                    )
                    .OrderBy(x => x.CreatedDate)
                    .Skip(request.Request.Start)
                    .Take(request.Request.Length)
                    .ToListAsync(cancellationToken);
            } else
            {
                images = await _context.Images.AsNoTracking()
                .Where(x =>
                    (value == null ? true : x.Title.Contains(value))
                    && (!x.CreatedDate.HasValue
                        || ((!request.Request.StartDateTime.HasValue || request.Request.StartDateTime.Value <= x.CreatedDate.Value)
                            && (!request.Request.EndDateTime.HasValue || request.Request.EndDateTime.Value >= x.CreatedDate.Value)))
                )
                .OrderByDescending(order)
                .Skip(request.Request.Start)
                .Take(request.Request.Length)
                .ToListAsync(cancellationToken);
            }

            var max = count / request.Request.Length + (count % request.Request.Length == 0 ? 0 : 1);
            var current = (request.Request.Start + 1) / request.Request.Length + ((request.Request.Start + 1) % request.Request.Length == 0 ? 0 : 1);

            var paging = new PagingResponse<ImageEntity>
            {
                List = images,
                PerPage = request.Request.Length,
                Max = max,
                Current = current,
            };

            return DataResponse<PagingResponse<ImageEntity>>.Success(paging);

        }
    }
}