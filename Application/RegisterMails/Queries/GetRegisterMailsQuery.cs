using Application.Common.Interfaces;
using Application.Common.Requests;
using Application.Common.Responses;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.RegisterMails.Queries;

public class GetRegisterMailsQuery : IRequest<DataResponse<PagingResponse<RegisterMailEntity>>>
{
    public SearchRequest Request { get; }

    public GetRegisterMailsQuery(SearchRequest request)
    {
        Request = request;
    }

    public class Handler : IRequestHandler<GetRegisterMailsQuery, DataResponse<PagingResponse<RegisterMailEntity>>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResponse<PagingResponse<RegisterMailEntity>>> Handle(GetRegisterMailsQuery request, CancellationToken cancellationToken)
        {
            var value = request.Request.Value;
            // Lấy số lượng ảnh được lọc theo yêu cầu
            var count = await _context.RegisterMails.AsNoTracking()
                .Where(x =>
                    (value == null ? true : x.Email.Contains(value))
                    && (!x.CreatedDate.HasValue
                        || ((!request.Request.StartDate.HasValue || request.Request.StartDate.Value <= x.CreatedDate.Value)
                            && (!request.Request.EndDate.HasValue || request.Request.EndDate.Value >= x.CreatedDate.Value)))
                ).CountAsync(cancellationToken);

            // Order
            Expression<Func<RegisterMailEntity, Object?>> order = null;
            var isAsc = request.Request.IsAsc ?? true;
            switch (request.Request.Order)
            {
                case "date":
                default:
                    order = item => item.CreatedDate;
                    break;
            }

            // Lấy danh sách theo phân trang
            if (request.Request.Start >= count)
            {
                request.Request.Start = 0;
            }
            List<RegisterMailEntity> registerMails;
            if (isAsc)
            {
                registerMails = await _context.RegisterMails.AsNoTracking()
                    .Where(x =>
                        (value == null ? true : x.Email.Contains(value))
                        && (!x.CreatedDate.HasValue
                            || ((!request.Request.StartDate.HasValue || request.Request.StartDate.Value <= x.CreatedDate.Value)
                                && (!request.Request.EndDate.HasValue || request.Request.EndDate.Value >= x.CreatedDate.Value)))
                    )
                    .OrderBy(x => x.CreatedDate)
                    .Skip(request.Request.Start)
                    .Take(request.Request.Length)
                    .ToListAsync(cancellationToken);
            }
            else
            {
                registerMails = await _context.RegisterMails.AsNoTracking()
                .Where(x =>
                    (value == null ? true : x.Email.Contains(value))
                    && (!x.CreatedDate.HasValue
                        || ((!request.Request.StartDate.HasValue || request.Request.StartDate.Value <= x.CreatedDate.Value)
                            && (!request.Request.EndDate.HasValue || request.Request.EndDate.Value >= x.CreatedDate.Value)))
                )
                .OrderByDescending(order)
                .Skip(request.Request.Start)
                .Take(request.Request.Length)
                .ToListAsync(cancellationToken);
            }

            var max = count / request.Request.Length + (count % request.Request.Length == 0 ? 0 : 1);
            var current = (request.Request.Start + 1) / request.Request.Length + ((request.Request.Start + 1) % request.Request.Length == 0 ? 0 : 1);

            var paging = new PagingResponse<RegisterMailEntity>
            {
                List = registerMails,
                PerPage = request.Request.Length,
                Max = max,
                Current = current,
            };

            return DataResponse<PagingResponse<RegisterMailEntity>>.Success(paging);

        }
    }
}