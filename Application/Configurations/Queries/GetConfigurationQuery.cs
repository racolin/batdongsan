using Application.Common.Interfaces;
using Application.Common.Requests;
using Application.Common.Responses;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.Configurations.Queries;

public class GetConfigurationQuery : IRequest<DataResponse<ConfigurationEntity>>
{
    public int Id { get; }

    public GetConfigurationQuery(int id)
    {
        Id = id;
    }

    public class Handler : IRequestHandler<GetConfigurationQuery, DataResponse<ConfigurationEntity>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResponse<ConfigurationEntity>> Handle(GetConfigurationQuery request, CancellationToken cancellationToken)
        {
            var cofiguration = await _context.Configurations.AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            if (cofiguration == null)
            {
                return DataResponse<ConfigurationEntity>.Error("Không tìm thấy cấu hình!");
            }

            return DataResponse<ConfigurationEntity>.Success(cofiguration);

        }
    }
}