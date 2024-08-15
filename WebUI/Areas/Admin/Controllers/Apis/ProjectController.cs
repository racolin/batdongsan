using Application.Common.Requests;
using Application.Common.Responses;
using Application.Project.Commands;
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
    }
}
