using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Application.Common.Responses;
using Domain.Entities;

namespace Application.Contents.Queries;

public class GetContentQuery : IRequest<DataResponse<ContentEntity>>
{
    public int Id { get; }

    public GetContentQuery(int id)
    {
        Id = id;
    }

    public class Handler : IRequestHandler<GetContentQuery, DataResponse<ContentEntity>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResponse<ContentEntity>> Handle(GetContentQuery request, CancellationToken cancellationToken)
        {
            var content = await _context.Contents.AsNoTracking()
                .Include(x => x.BgHomeImage)
                .Include(x => x.HomeImage)
                .Include(x => x.ContactImage)
                .Include(x => x.NewsImage)
                .Include(x => x.ProjectSlider.OrderBy(y => y.Order)).ThenInclude(x => x.Image)
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            if (content == null)
            {
                return DataResponse<ContentEntity>.Error("Không tìm thấy nội dung này!");
            }

            return DataResponse<ContentEntity>.Success(content);

        }
    }
}