using Application.Common.Interfaces;
using Application.Common.Requests;
using Application.Common.Responses;
using Domain.Constants;
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
            var status = request.Request.Status.Count > 0 ? request.Request.Status : StatusConstant.GetAllProperties();
            var highLight = request.Request.HighLight.Count > 0 ? request.Request.HighLight : new List<bool> { true, false };
            var type = request.Request.Type.Count > 0 ? request.Request.Type : ProjectTypeConstant.GetAllProperties();
            var state = request.Request.State.Count > 0 ? request.Request.State : ProjectStateConstant.GetAllProperties();
            var address = request.Request.ValueFilter1;
            var value = request.Request.Value;

            // Lấy số lượng dự án được lọc theo yêu cầu
            var count = await _context.Projects.AsNoTracking()
                .Where(x =>
                    (value == null ? true : x.Name.Contains(value))
                    &&(address == null ? true : x.Address.Contains(address))
                    && (!x.CreatedDate.HasValue
                        || ((!request.Request.StartDateTime.HasValue || request.Request.StartDateTime.Value <= x.CreatedDate.Value)
                            && (!request.Request.EndDateTime.HasValue || request.Request.EndDateTime.Value >= x.CreatedDate.Value)))
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

            if (count < request.Request.Start)
            {
                request.Request.CurrentPage = 0;
            }

            List<ProjectEntity> projects;
            if (isAsc)
            {
                projects = await _context.Projects.AsNoTracking()
                    .Include(x => x.Image)
                    .Where(x =>
                        (value == null ? true : x.Name.Contains(value))
                        && (address == null ? true : x.Address.Contains(address))
                        && (!x.CreatedDate.HasValue
                            || ((!request.Request.StartDateTime.HasValue || request.Request.StartDateTime.Value <= x.CreatedDate.Value)
                                && (!request.Request.EndDateTime.HasValue || request.Request.EndDateTime.Value >= x.CreatedDate.Value)))
                        && state.Contains(x.State)
                        && type.Contains(x.Type)
                        && highLight.Contains(x.IsHighlight)
                        && status.Contains(x.Status)
                    )
                    .OrderBy(order)
                    .Skip(request.Request.Start)
                    .Take(request.Request.Length)
                    .ToListAsync(cancellationToken);
            }
            else
            {
                projects = await _context.Projects.AsNoTracking()
                .Include(x => x.Image)
                .Where(x =>
                    (value == null ? true : x.Name.Contains(value))
                    && (address == null ? true : x.Address.Contains(address))
                    && (!x.CreatedDate.HasValue
                        || ((!request.Request.StartDateTime.HasValue || request.Request.StartDateTime.Value <= x.CreatedDate.Value)
                            && (!request.Request.EndDateTime.HasValue || request.Request.EndDateTime.Value >= x.CreatedDate.Value)))
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
                List = projects,
                PerPage = request.Request.Length,
                Max = max,
                Current = current,
            };

            return DataResponse<PagingResponse<ProjectEntity>>.Success(paging);

        }
    }
}