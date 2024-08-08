using Application.Common.Interfaces;
using Application.Common.Requests;
using Application.Common.Responses;
using Application.Common.Responses.Admin;
using Application.Common.Supports;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Images.Queries;

public class GetShortImagesQuery : IRequest<DataResponse<PagingResponse<ShortImageResponse>>>
{
    public SearchRequest Request { get; }

    public GetShortImagesQuery(SearchRequest request)
    {
        Request = request;
    }

    public class Handler : IRequestHandler<GetShortImagesQuery, DataResponse<PagingResponse<ShortImageResponse>>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResponse<PagingResponse<ShortImageResponse>>> Handle(GetShortImagesQuery request, CancellationToken cancellationToken)
        {
            var value = request.Request.Value;
            // Lấy số lượng ảnh được lọc theo yêu cầu
            var count = await _context.Images.AsNoTracking()
                .Where(x => value == null ? true : x.Title.Contains(request.Request.Value ?? ""))
                .CountAsync(cancellationToken);

            // Lấy danh sách theo phân trang
            if (request.Request.Start >= count) {
                request.Request.Start = 0;
            }
            var images = await _context.Images.AsNoTracking()
                .Select(x => new ShortImageResponse
                {
                    Id = x.Id,
                    Title = x.Title,
                    Name = PathSupport.GetUploadThumbDefaultPath(x.Name, x.Type),
                })
                .Where(x => value == null ? true : x.Title.Contains(request.Request.Value ?? ""))
                .OrderByDescending(x => x.Id)
                .Skip(request.Request.Start)
                .Take(request.Request.Length)
                .ToListAsync(cancellationToken);

            var max = count / request.Request.Length + (count % request.Request.Length == 0 ? 0 : 1);
            var current = (request.Request.Start + 1) / request.Request.Length + ((request.Request.Start + 1) % request.Request.Length == 0 ? 0 : 1);

            var paging = new PagingResponse<ShortImageResponse>
            {
                List = images,
                PerPage = request.Request.Length,
                Max = max,
                Current = current,
            };

            return DataResponse<PagingResponse<ShortImageResponse>>.Success(paging);

        }
    }
}