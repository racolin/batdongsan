using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;
using Application.Common.Requests;
using Application.Common.Responses;
using Domain.Entities;
using Domain.Enums;

namespace Application.Images.Commands;
public class SaveImageCommand : IRequest<DataResponse<int>>
{
    public SaveImageRequest Request { get; }

    public SaveImageCommand(SaveImageRequest request)
    {
        Request = request;
    }

    public class Handler : IRequestHandler<SaveImageCommand, DataResponse<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public Handler(IApplicationDbContext context, IFileService fileService, IMapper mapper)
        {
            _context = context;
            _fileService = fileService;
            _mapper = mapper;
        }
        private bool CheckExt(string filename1, string filename2)
        {
            return filename1.Split(".").Last() == filename2.Split(".").Last();
        }

        public async Task<DataResponse<int>> Handle(SaveImageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var image = await _context.Images.FirstOrDefaultAsync(x => x.Id == request.Request.Id, cancellationToken);
                if (image == null && request.Request.Id != null)
                {
                    return DataResponse<int>.Error("Không tìm thấy ảnh để cập nhật!");
                }

                if (image == null)
                {
                    image = _mapper.Map<ImageEntity>(request.Request);
                    image.Name = await _fileService.SaveFileCommand(request.Request.Large!, "images", request.Request.Name);
                    if (image.Type == (int)ImageTypeEnum.HasThumb)
                    {
                        await _fileService.SaveFileCommand(request.Request.Small!, "thumbs", image.Name.Split(".").First());
                    }
                    _context.Images.Add(image);
                }
                else
                {
                    // Định dạng ảnh phải giống nhau hết
                    // Nếu từ lớn chuyển sang lớn và thu nhỏ thì phải đưa ảnh nhỏ lên
                    if (request.Request.Type == (int)ImageTypeEnum.HasThumb)
                    {
                        if (!CheckExt(request.Request.Large?.FileName ?? image.Name, request.Request.Small?.FileName ?? image.Name))
                        {
                            return DataResponse<int>.Error("Định dạng của ảnh và ảnh thu nhỏ phải giống nhau!");
                        }

                        if (image.Type == (int)ImageTypeEnum.OnlyFull && request.Request.Small == null)
                        {
                            return DataResponse<int>.Error("Phải đính kèm ảnh thu nhỏ!");
                        }
                    }

                    // Both => Only
                    // Xóa ảnh thu nhỏ nếu cái cũ là cả hai mà cái mới chỉ một
                    if (request.Request.Type == (int)ImageTypeEnum.OnlyFull && image.Type == (int)ImageTypeEnum.HasThumb)
                    {
                        var deleted = _fileService.DeleteFileCommand(image.Name, "thumbs");
                    }

                    var newFileName = "";

                    // Only => Both
                    // Small
                    // Có đưa ảnh thu nhỏ lên (đồng thời cái loại mới là cả hai) thì xóa ảnh cũ (nếu có) và lưu ảnh mới với tên mới
                    if (request.Request.Small != null && request.Request.Type == (int)ImageTypeEnum.HasThumb)
                    {
                        var deleted = _fileService.DeleteFileCommand(image.Name, "thumbs");
                        newFileName = await _fileService.SaveFileCommand(request.Request.Small!, "thumbs", request.Request.Name);
                    }

                    // Large
                    // Có đưa ảnh lên thì xóa ảnh cũ và lưu ảnh mới với tên mới
                    if (request.Request.Large != null)
                    {
                        var deleted = _fileService.DeleteFileCommand(image.Name, "images");
                        newFileName = await _fileService.SaveFileCommand(request.Request.Large!, "images", request.Request.Name);
                    }

                    if (!string.IsNullOrEmpty(newFileName))
                    {
                        // Nếu đã có một cái đổi - tức là newFileName khác null thì sẽ cập nhật cái còn lại nếu có thể
                        if (request.Request.Large == null)
                        {
                            newFileName = _fileService.RenameFileCommand("images", image.Name, newFileName);
                            if (newFileName == null)
                            {
                                return DataResponse<int>.Error("Ảnh đã bị xóa không đúng cách, hãy liên hệ admin!");
                            }
                        }
                        if (request.Request.Small == null && request.Request.Type == (int)ImageTypeEnum.HasThumb)
                        {
                            newFileName = _fileService.RenameFileCommand("thumbs", image.Name, newFileName);
                            if (newFileName == null)
                            {
                                return DataResponse<int>.Error("Ảnh thu nhỏ đã bị xóa không đúng cách, hãy liên hệ admin!");
                            }
                        }
                    }
                    else
                    {
                        // Nếu chưa cái nào đổi cả mà tên đưa lên khác với tên gốc thì tiến hành đổi tên
                        if (!image.Name.Split('.').First().Equals(request.Request.Name))
                        {
                            newFileName = _fileService.RenameFileCommand("images", image.Name, request.Request.Name + "." + image.Name.Split(".").Last());
                            if (newFileName == null)
                            {
                                return DataResponse<int>.Error("Ảnh đã bị xóa không đúng cách, hãy liên hệ admin!");
                            }
                            if (request.Request.Type == (int)ImageTypeEnum.HasThumb)
                            {
                                newFileName = _fileService.RenameFileCommand("thumbs", image.Name, newFileName!);
                                if (newFileName == null)
                                {
                                    return DataResponse<int>.Error("Ảnh thu nhỏ đã bị xóa không đúng cách, hãy liên hệ admin!");
                                }
                            }
                        }
                    }

                    image.Name = string.IsNullOrEmpty(newFileName) ? image.Name : newFileName;
                    image.Type = request.Request.Type;
                    image.Alt = request.Request.Alt ?? image.Alt;
                    image.Title = request.Request.Title ?? image.Title;
                    image.Link = request.Request.Link;
                }

                await _context.SaveChangesAsync(cancellationToken);

                return new DataResponse<int> { Data = image.Id };
            }
            catch (Exception ex) {
                return DataResponse<int>.Error(ex.Message);
            }
        }
    }
}