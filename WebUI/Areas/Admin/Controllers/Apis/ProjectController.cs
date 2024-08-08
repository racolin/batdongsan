using Application.Common.Requests;
using Application.Common.Responses;
using Application.Project.Commands;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers.Apis
{
    [ApiExplorerSettings(GroupName = "Project - Admin")]
    public class ProjectController : ApiAdminControllerBase
    {
        [HttpPost("save")]
        public async Task<DataResponse<int>> Save(SaveProjectRequest request)
        {
            var result = await Mediator.Send(new SaveProjectCommand(request));
            return result ?? DataResponse<int>.Error("Có lỗi khi lưu dữ liệu!");
        }
    }
}
