using Application.Common.Requests;
using Application.Common.Responses;
using Application.RegisterMails.Commands;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers.Apis
{
    [ApiExplorerSettings(GroupName = "RegisterMail - Admin")]
    public class RegisterMailController : ApiAdminControllerBase
    {
        [HttpPost("save")]
        public async Task<DataResponse<int>> Save(SaveRegisterMailRequest request)
        {
            var result = await Mediator.Send(new SaveRegisterMailCommand(request));
            return result ?? DataResponse<int>.Error("Có lỗi khi lưu dữ liệu!");
        }
    }
}
