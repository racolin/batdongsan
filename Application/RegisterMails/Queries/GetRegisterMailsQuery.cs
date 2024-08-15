using Application.Common.Interfaces;
using Application.Common.Requests;
using Application.Common.Responses;
using Domain.Constants;
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
            var state = request.Request.State.Count > 0 ? 
                request.Request.State : 
                [
                    RegisterMailStateConstant.Sent, 
                    RegisterMailStateConstant.Added, 
                    RegisterMailStateConstant.Expired
                ];
            var value = request.Request.Value;
            // Lấy số lượng ảnh được lọc theo yêu cầu
            var count = await _context.RegisterMails.AsNoTracking()
                .Where(x =>
                    (value == null ? true : x.Email.Contains(value))
                    && (!x.CreatedDate.HasValue
                        || ((!request.Request.StartDateTime.HasValue || request.Request.StartDateTime.Value <= x.CreatedDate.Value)
                            && (!request.Request.EndDateTime.HasValue || request.Request.EndDateTime.Value >= x.CreatedDate.Value)))
                    && state.Contains(x.State)
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

            if (count < request.Request.Start)
            {
                request.Request.CurrentPage = 0;
            }
            List<RegisterMailEntity> registerMails;
            if (isAsc)
            {
                registerMails = await _context.RegisterMails.AsNoTracking()
                    .Where(x =>
                        (value == null ? true : x.Email.Contains(value))
                        && (!x.CreatedDate.HasValue
                            || ((!request.Request.StartDateTime.HasValue || request.Request.StartDateTime.Value <= x.CreatedDate.Value)
                                && (!request.Request.EndDateTime.HasValue || request.Request.EndDateTime.Value >= x.CreatedDate.Value)))
                        && state.Contains(x.State)
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
                        || ((!request.Request.StartDateTime.HasValue || request.Request.StartDateTime.Value <= x.CreatedDate.Value)
                            && (!request.Request.EndDateTime.HasValue || request.Request.EndDateTime.Value >= x.CreatedDate.Value)))
                    && state.Contains(x.State)
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