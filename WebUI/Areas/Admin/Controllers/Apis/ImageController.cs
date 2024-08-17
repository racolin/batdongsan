using Application.Common.Requests;
using Application.Common.Responses;
using Application.Common.Responses.Admin;
using Application.Images.Commands;
using Application.Images.Queries;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers.Apis
{
    [ApiExplorerSettings(GroupName = "Image - Admin")]
    public class ImageController : ApiAdminControllerBase
    {
        [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.ImagePoster}")]
        [HttpPost("save")]
        public async Task<DataResponse<int>> Save([FromForm] SaveImageRequest request)
        {
            var result = await Mediator.Send(new SaveImageCommand(request));
            return result ?? DataResponse<int>.Error("Có lỗi khi lưu dữ liệu!");
        }
        [Authorize]
        [HttpGet("get-short-images")]
        public async Task<DataResponse<PagingResponse<ShortImageResponse>>> GetShortImages(string? value, int? perPage, int? currentPage)
        {
            var result = await Mediator.Send(
                new GetShortImagesQuery(
                    new SearchRequest { 
                        Value = value, 
                        PerPage = perPage ?? 16, 
                        CurrentPage = currentPage,
                    })
                );
            return result ?? DataResponse<PagingResponse<ShortImageResponse>>.Error("Có lỗi khi tải dữ liệu!");
        }
        [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.ImagePoster}")]
        [HttpGet("get-references")]
        public async Task<DataResponse<List<ReferencesResponse>>> GetReferences(int id)
        {
            var result = await Mediator.Send(new GetReferencesOfImageQuery(id));
            return result ?? DataResponse<List<ReferencesResponse>>.Error("Có lỗi khi tải dữ liệu!");
        }
        [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.ImagePoster}")]
        [HttpGet("delete")]
        public async Task<DataResponse<bool>> Delete(int id)
        {
            var result = await Mediator.Send(new DeleteImageCommand(id));
            return result ?? DataResponse<bool>.Error("Có lỗi khi xóa ảnh!");
        }
    }
}
