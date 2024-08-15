using Application.Common.Requests;
using Application.Common.Responses;
using Application.RegisterMails.Commands;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers.Apis
{
    [ApiExplorerSettings(GroupName = "RegisterMail - Admin")]
    public class RegisterMailController : ApiAdminControllerBase
    {
        [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.RegisteredEmailChecker}")]
        [HttpPost("save")]
        public async Task<DataResponse<int>> Save(SaveRegisterMailRequest request)
        {
            var result = await Mediator.Send(new SaveRegisterMailCommand(request));
            return result ?? DataResponse<int>.Error("Có lỗi khi lưu dữ liệu!");
        }
        [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.Caller}")]
        [HttpPost("update-state")]
        public async Task<DataResponse<bool>> UpdateState(UpdateStateRequest request)
        {
            var result = await Mediator.Send(new UpdateStateRegisterMailCommand(request));
            return result ?? DataResponse<bool>.Error("Có lỗi khi lưu dữ liệu!");
        }
        [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.Caller}")]
        [HttpGet("remove")]
        public async Task<DataResponse<bool>> Remove(int id)
        {
            var result = await Mediator.Send(new RemoveRegisterMailCommand(id));
            return result ?? DataResponse<bool>.Error("Có lỗi khi xóa!");
        }
        [Authorize(Roles = $"{RoleConstant.Admin}")]
        [HttpGet("delete")]
        public async Task<DataResponse<bool>> Delete(int id)
        {
            var result = await Mediator.Send(new DeleteRegisterMailCommand(id));
            return result ?? DataResponse<bool>.Error("Có lỗi khi lưu dữ liệu!");
        }
    }
}
