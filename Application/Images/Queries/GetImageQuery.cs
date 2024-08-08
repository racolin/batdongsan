using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Application.Common.Responses;
using Domain.Entities;

namespace Application.Images.Queries;

public class GetImageQuery : IRequest<DataResponse<ImageEntity>>
{
    public int Id { get; }

    public GetImageQuery(int id)
    {
        Id = id;
    }

    public class Handler : IRequestHandler<GetImageQuery, DataResponse<ImageEntity>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResponse<ImageEntity>> Handle(GetImageQuery request, CancellationToken cancellationToken)
        {
            ImageEntity? image = null;
            try
            {
                image = await _context.Images.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            }
            catch (Exception ex) { 
                return DataResponse<ImageEntity>.Error("Có lỗi xảy ra với dữ liệu, hãy liên hệ admin để xử lý!", [ex.Message]);
            }
            if (image == null) {
                return DataResponse<ImageEntity>.Error("Không tìm thấy ảnh. Có thể ảnh đã bị xóa hoặc thậm chí chưa được tạo!");
            }
            return DataResponse<ImageEntity>.Success(image);
        }
    }
}