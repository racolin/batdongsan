using Application.Common.Interfaces;
using MediatR;
using Application.Common.Responses;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Images.Commands;

public class DeleteImageCommand : IRequest<DataResponse<bool>>
{
    public int Id { get; }
    public DeleteImageCommand(int id)
    {
        Id = id;
    }

    public class Handler : IRequestHandler<DeleteImageCommand, DataResponse<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IFileService _fileService;

        public Handler(IApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<DataResponse<bool>> Handle(DeleteImageCommand request, CancellationToken cancellationToken)
        {
            var id = request.Id;
            var image = await _context.Images.FindAsync(id);
            if (image == null)
            {
                return DataResponse<bool>.Error("Không tìm thấy ảnh muốn xóa!");
            }

            //Project
            var isInProjects = await _context.Projects.AsNoTracking()
                .AnyAsync(x => x.ImageId == request.Id, cancellationToken);
            if (isInProjects)
            {
                return DataResponse<bool>.Error("Ảnh được sử dụng ở dự án nên không thể xóa!");
            }

            //News
            var isInNewsList = await _context.News.AsNoTracking()
                .AnyAsync(x => x.ImageId == request.Id, cancellationToken);
            if (isInNewsList)
            {
                return DataResponse<bool>.Error("Ảnh được sử dụng ở tin tức nên không thể xóa!");
            }

            //Content
            var isInContents = await _context.Contents.AsNoTracking()
                .Include(x => x.ProjectSlider)
                .AnyAsync(x =>
                    x.HomeImageId == request.Id
                    || x.BgHomeImageId == request.Id
                    || x.NewsImageId == request.Id
                    || x.ContactImageId == request.Id
                    || x.ProjectSlider.Any(x => x.ImageId == request.Id)
                , cancellationToken);
            if (isInContents)
            {
                return DataResponse<bool>.Error("Ảnh được sử dụng ở nội dug web nên không thể xóa!");
            }

            _fileService.DeleteFileCommand(image.Name, "images");

            if (image.Type == (int)ImageTypeEnum.HasThumb)
            {
                _fileService.DeleteFileCommand(image.Name, "thumbs");
            }

            _context.Images.Remove(image);

            await _context.SaveChangesAsync(cancellationToken);

            return DataResponse<bool>.Success(true);
        }

    }
}