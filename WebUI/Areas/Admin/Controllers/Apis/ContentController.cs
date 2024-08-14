using Application.Common.Requests;
using Application.Common.Responses;
using Application.Contents.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers.Apis
{
    [ApiExplorerSettings(GroupName = "Content - Admin")]
    public class ContentController : ApiAdminControllerBase
    {
        [AllowAnonymous]
        [HttpPost("save")]
        public async Task<DataResponse<int>> Save(SaveContentRequest request)
        {
            var result = await Mediator.Send(new SaveContentCommand(request));
            return result ?? DataResponse<int>.Error("Có lỗi khi lưu dữ liệu!");
        }
    }
}
