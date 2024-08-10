using Application.Account.Commands;
using Application.Common.Requests;
using Application.Common.Responses;
using Application.Users.Commands;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers.Apis
{
    [ApiExplorerSettings(GroupName = "User - Admin")]
    public class UserController : ApiAdminControllerBase
    {
        [HttpPost("update-password")]
        public async Task<DataResponse<bool>> UpdatePassword(UpdatePasswordRequest request)
        {
            var result = await Mediator.Send(new UpdatePasswordCommand(request));
            return result ?? DataResponse<bool>.Error("Có lỗi khi đặt mật khẩu!");
        }
        [HttpPost("save")]
        public async Task<DataResponse<int>> Save([FromForm] SaveUserRequest request)
        {
            var result = await Mediator.Send(new SaveUserCommand(request));
            return result ?? DataResponse<int>.Error("Có lỗi khi lưu người dùng!");
        }
        [HttpPost("update-my-password")]
        public async Task<DataResponse<bool>> UpdateMyPassword(UpdateMyPasswordRequest request)
        {
            var result = await Mediator.Send(new UpdateMyPasswordCommand(request));
            return result ?? DataResponse<bool>.Error("Có lỗi khi đặt mật khẩu!");
        }
        [HttpPost("update-profile")]
        public async Task<DataResponse<bool>> UpdateProfile([FromForm] SaveUserRequest request)
        {
            var result = await Mediator.Send(new UpdateProfileCommand(request));
            return result ?? DataResponse<bool>.Error("Có lỗi khi lưu người dùng!");
        }
        [HttpPost("add-role")]
        public async Task<DataResponse<bool>> AddRole(UpdateRoleRequest request)
        {
            var result = await Mediator.Send(new UpdateRoleCommand(request, true));
            return result ?? DataResponse<bool>.Error("Có lỗi khi thêm vai trò này!");
        }
        [HttpPost("remove-role")]
        public async Task<DataResponse<bool>> RemoveRole(UpdateRoleRequest request)
        {
            var result = await Mediator.Send(new UpdateRoleCommand(request, false));
            return result ?? DataResponse<bool>.Error("Có lỗi khi xóa vai trò này!");
        }
        [HttpPost("set-lock")]
        public async Task<DataResponse<bool>> SetLock(SetLockUserRequest request)
        {
            var result = await Mediator.Send(new SetLockUserCommand(request));
            return result ?? DataResponse<bool>.Error("Có lỗi khi " + (request.IsLock ? "khóa" : "mở khóa cho") + " người dùng này!");
        }

    }
}
