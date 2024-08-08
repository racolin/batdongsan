using Application.Common.Requests;
using Application.Common.Responses;
using Application.Images.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers.Apis
{
    [ApiExplorerSettings(GroupName = "Content - Admin")]
    public class ContentController : ApiAdminControllerBase
    {
        [AllowAnonymous]
        [HttpPost("save")]
        public async Task<DataResponse<bool>> Save(SaveContentRequest request)
        {
            var result = await Mediator.Send(new SaveContentCommand(request));
            return result ?? DataResponse<bool>.Error("Có lỗi khi lưu dữ liệu!");
        }
    }
}
