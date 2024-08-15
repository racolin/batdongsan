using Application.Common.Interfaces;
using Application.Common.Requests;
using Application.Common.Responses;
using Domain.Constants;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.Contacts.Queries;

public class GetContactsQuery : IRequest<DataResponse<PagingResponse<ContactEntity>>>
{
    public SearchRequest Request { get; }

    public GetContactsQuery(SearchRequest request)
    {
        Request = request;
    }

    public class Handler : IRequestHandler<GetContactsQuery, DataResponse<PagingResponse<ContactEntity>>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResponse<PagingResponse<ContactEntity>>> Handle(GetContactsQuery request, CancellationToken cancellationToken)
        {
            var state = request.Request.State.Count > 0 ? 
                request.Request.State : 
                new List<string> { 
                    ContactStateConstant.Sent, 
                    ContactStateConstant.Processed, 
                    ContactStateConstant.Refused, 
                };
            var value = request.Request.Value;
            // Lấy số lượng ảnh được lọc theo yêu cầu
            var count = await _context.Contacts.AsNoTracking()
                .Where(x =>
                    (value == null ? true : x.Email.Contains(value))
                    && (!x.CreatedDate.HasValue
                        || ((!request.Request.StartDateTime.HasValue || request.Request.StartDateTime.Value <= x.CreatedDate.Value)
                            && (!request.Request.EndDateTime.HasValue || request.Request.EndDateTime.Value >= x.CreatedDate.Value)))
                    && state.Contains(x.State)
                ).CountAsync(cancellationToken);

            // Order
            Expression<Func<ContactEntity, Object?>> order = null;
            var isAsc = request.Request.IsAsc ?? true;
            switch (request.Request.Order)
            {
                case "date":
                default:
                    order = item => item.CreatedDate;
                    break;
            }

            // Lấy danh sách theo phân trang
            if (count < request.Request.Start)
            {
                request.Request.CurrentPage = 0;
            }
            List<ContactEntity> contacts;
            if (isAsc)
            {
                contacts = await _context.Contacts.AsNoTracking()
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
                contacts = await _context.Contacts.AsNoTracking()
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

            var paging = new PagingResponse<ContactEntity>
            {
                List = contacts,
                PerPage = request.Request.Length,
                Max = max,
                Current = current,
            };

            return DataResponse<PagingResponse<ContactEntity>>.Success(paging);

        }
    }
}