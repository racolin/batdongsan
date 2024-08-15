using Application.Common.Requests;
using Application.Common.Responses;
using Application.Contacts.Commands;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers.Apis
{
    [ApiExplorerSettings(GroupName = "Contact - Admin")]
    public class ContactController : ApiAdminControllerBase
    {
        [AllowAnonymous]
        [HttpPost("save")]
        public async Task<DataResponse<int>> Save(SaveContactRequest request)
        {
            var result = await Mediator.Send(new SaveContactCommand(request));
            return result ?? DataResponse<int>.Error("Có lỗi khi lưu dữ liệu!");
        }
        [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.Caller}")]
        [HttpPost("update")]
        public async Task<DataResponse<bool>> Update(UpdateContactRequest request)
        {
            var result = await Mediator.Send(new UpdateContactCommand(request));
            return result ?? DataResponse<bool>.Error("Có lỗi khi lưu dữ liệu!");
        }
        [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.Caller}")]
        [HttpPost("update-state")]
        public async Task<DataResponse<bool>> UpdateState(UpdateStateRequest request)
        {
            var result = await Mediator.Send(new UpdateStateContactCommand(request));
            return result ?? DataResponse<bool>.Error("Có lỗi khi lưu dữ liệu!");
        }
        [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.Caller}")]
        [HttpGet("remove")]
        public async Task<DataResponse<bool>> Remove(int id)
        {
            var result = await Mediator.Send(new RemoveContactCommand(id));
            return result ?? DataResponse<bool>.Error("Có lỗi khi xóa!" + id.ToString());
        }
        [Authorize(Roles = $"{RoleConstant.Admin}")]
        [HttpGet("delete")]
        public async Task<DataResponse<bool>> Delete(int id)
        {
            var result = await Mediator.Send(new DeleteContactCommand(id));
            return result ?? DataResponse<bool>.Error("Có lỗi khi lưu dữ liệu!");
        }
    }
}
