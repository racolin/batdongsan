using Application.Common.Requests;
using Application.Common.Responses;
using Application.Contacts.Commands;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers.Apis
{
    [ApiExplorerSettings(GroupName = "Contact - Admin")]
    public class ContactController : ApiAdminControllerBase
    {
        [HttpPost("save")]
        public async Task<DataResponse<int>> Save(SaveContactRequest request)
        {
            var result = await Mediator.Send(new SaveContactCommand(request));
            return result ?? DataResponse<int>.Error("Có lỗi khi lưu dữ liệu!");
        }
    }
}
