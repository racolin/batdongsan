using Application.Common.Requests;
using Application.Common.Responses;
using Application.Common.Responses.Admin;
using Application.Users.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers.Apis
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AuthController : ApiAdminControllerBase
    {
        [AllowAnonymous]
        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<DataResponse<LoginResponse>> Login([FromForm] LoginRequest request)
        {
            var response = await Mediator.Send(new LoginCommand(request));
            return response;
        }

        [HttpGet("logout")]
        public async Task<DataResponse<bool>> Logout()
        {
            var response = await Mediator.Send(new LogoutCommand());
            return response;
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        [ValidateAntiForgeryToken]
        public async Task<DataResponse<bool>> ForgotPassword([FromForm] string username)
        {
            var platform = Request.Headers["sec-ch-ua-platform"].ToString();
            var browser = "Unknown";
            var brs = Request.Headers["sec-ch-ua"].ToString().Split(",");
            if (brs.Length > 1)
            {
                var brss = brs[1].Split(";");
                if (brss.Length > 0)
                {
                    browser = brss[0];
                }
            }
            var response = await Mediator.Send(new ForgotPasswordCommand(username, platform, browser));
            return response;
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        [ValidateAntiForgeryToken]
        public async Task<DataResponse<bool>> ResetPassword([FromForm] ResetPasswordRequest request)
        {
            var response = await Mediator.Send(new ResetPasswordCommand(request));
            return response;
        }
    }
}
