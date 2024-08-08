using Application.Contacts.Commands;
using Application.Common.Requests;
using Application.Common.Responses;
using Microsoft.AspNetCore.Mvc;
using Application.RegisterMails.Commands;

namespace WebUI.Controllers.Apis
{
    [ApiExplorerSettings(GroupName = "Contact - Client")]
    public class ContactController : ApiControllerBase
    {
        [HttpPost("send-contact")]
        [AutoValidateAntiforgeryToken]
        public async Task<DataResponse<int>> Save([FromForm] SaveContactRequest request)
        {
            request.Id = null;
            request.State = null;
            var result = await Mediator.Send(new SaveContactCommand(request));
            return result ?? DataResponse<int>.Error("Có lỗi đã xảy ra khi thực hiện api này!");
        }
        [HttpPost("register-mail")]
        [AutoValidateAntiforgeryToken]
        public async Task<DataResponse<int>> RegisterMail([FromForm] SaveRegisterMailRequest request)
        {
            request.Id = null;
            request.State = null;
            var result = await Mediator.Send(new SaveRegisterMailCommand(request));
            return result ?? DataResponse<int>.Error("Có lỗi đã xảy ra khi thực hiện api này!");
        }
    }
}
