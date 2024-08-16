using Application.Common.Requests;
using Application.Common.Responses;
using Application.Project.Commands;
using Application.Projects.Commands;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers.Apis
{
    [ApiExplorerSettings(GroupName = "Project - Admin")]
    public class ProjectController : ApiAdminControllerBase
    {
        [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.ProjectPoster}")]
        [HttpPost("save")]
        public async Task<DataResponse<int>> Save(SaveProjectRequest request)
        {
            var result = await Mediator.Send(new SaveProjectCommand(request));
            return result ?? DataResponse<int>.Error("Có lỗi khi lưu dữ liệu!");
        }
        [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.ProjectPoster}")]
        [HttpPost("update-status")]
        public async Task<DataResponse<bool>> UpdateStatus(UpdateStatusRequest request)
        {
            var result = await Mediator.Send(new UpdateStatusProjectCommand(request));
            return result ?? DataResponse<bool>.Error("Có lỗi khi lưu dữ liệu!");
        }
        [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.ProjectPoster}")]
        [HttpGet("remove")]
        public async Task<DataResponse<bool>> Remove(int id)
        {
            var result = await Mediator.Send(new RemoveProjectCommand(id));
            return result ?? DataResponse<bool>.Error("Có lỗi khi xóa!" + id.ToString());
        }
        [Authorize(Roles = $"{RoleConstant.Admin}")]
        [HttpGet("delete")]
        public async Task<DataResponse<bool>> Delete(int id)
        {
            var result = await Mediator.Send(new DeleteProjectCommand(id));
            return result ?? DataResponse<bool>.Error("Có lỗi khi lưu dữ liệu!");
        }
    }
}
