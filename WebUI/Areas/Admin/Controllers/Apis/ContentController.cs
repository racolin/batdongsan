using Application.Common.Requests;
using Application.Common.Responses;
using Application.Contents.Commands;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers.Apis
{
    [Authorize(Roles = $"{RoleConstant.Admin}")]
    [ApiExplorerSettings(GroupName = "Content - Admin")]
    public class ContentController : ApiAdminControllerBase
    {
        [HttpPost("save")]
        public async Task<DataResponse<int>> Save(SaveContentRequest request)
        {
            var result = await Mediator.Send(new SaveContentCommand(request));
            return result ?? DataResponse<int>.Error("Có lỗi khi lưu dữ liệu!");
        }
        [HttpPost("update-status")]
        public async Task<DataResponse<bool>> UpdateStatus(UpdateStatusRequest request)
        {
            var result = await Mediator.Send(new UpdateStatusContentCommand(request));
            return result ?? DataResponse<bool>.Error("Có lỗi khi lưu dữ liệu!");
        }
        [HttpGet("remove")]
        public async Task<DataResponse<bool>> Remove(int id)
        {
            var result = await Mediator.Send(new RemoveContentCommand(id));
            return result ?? DataResponse<bool>.Error("Có lỗi khi xóa!" + id.ToString());
        }
        [HttpGet("delete")]
        public async Task<DataResponse<bool>> Delete(int id)
        {
            var result = await Mediator.Send(new DeleteContentCommand(id));
            return result ?? DataResponse<bool>.Error("Có lỗi khi lưu dữ liệu!");
        }
    }
}
