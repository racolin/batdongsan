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
        public async Task<DataResponse<bool>> Save([FromForm] SendContactRequest request)
        {
            var result = await Mediator.Send(new SendContactCommand(request, Request.HttpContext.Connection.RemoteIpAddress?.ToString()));
            return result ?? DataResponse<bool>.Error("Có lỗi đã xảy ra khi thực hiện api này!");
        }
        [HttpPost("register-mail")]
        [AutoValidateAntiforgeryToken]
        public async Task<DataResponse<bool>> RegisterMail([FromForm] SendRegisterMailRequest request)
        {
            var result = await Mediator.Send(new SendRegisterMailCommand(request, Request.HttpContext.Connection.RemoteIpAddress?.ToString()));
            return result ?? DataResponse<bool>.Error("Có lỗi đã xảy ra khi thực hiện api này!");
        }
    }
}
