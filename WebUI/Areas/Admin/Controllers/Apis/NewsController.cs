using Application.Common.Requests;
using Application.Common.Responses;
using Application.News.Commands;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers.Apis
{
    [ApiExplorerSettings(GroupName = "News - Admin")]
    public class NewsController : ApiAdminControllerBase
    {
        [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.NewsPoster}")]
        [HttpPost("save")]
        public async Task<DataResponse<int>> Save(SaveNewsRequest request)
        {
            var result = await Mediator.Send(new SaveNewsCommand(request));
            return result ?? DataResponse<int>.Error("Có lỗi khi lưu dữ liệu!");
        }
        [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.NewsPoster}")]
        [HttpPost("update-status")]
        public async Task<DataResponse<bool>> UpdateStatus(UpdateStatusRequest request)
        {
            var result = await Mediator.Send(new UpdateStatusNewsCommand(request));
            return result ?? DataResponse<bool>.Error("Có lỗi khi lưu dữ liệu!");
        }
        [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.NewsPoster}")]
        [HttpGet("remove")]
        public async Task<DataResponse<bool>> Remove(int id)
        {
            var result = await Mediator.Send(new RemoveNewsCommand(id));
            return result ?? DataResponse<bool>.Error("Có lỗi khi xóa!" + id.ToString());
        }
        [Authorize(Roles = $"{RoleConstant.Admin}")]
        [HttpGet("delete")]
        public async Task<DataResponse<bool>> Delete(int id)
        {
            var result = await Mediator.Send(new DeleteNewsCommand(id));
            return result ?? DataResponse<bool>.Error("Có lỗi khi lưu dữ liệu!");
        }
    }
}
