using Application.Common.Requests;
using Application.Common.Responses;
using Application.Configurations.Commands;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers.Apis
{
    [ApiExplorerSettings(GroupName = "Configuration - Admin")]
    public class ConfigurationController : ApiAdminControllerBase
    {
        [HttpPost("update")]
        public async Task<DataResponse<int>> Update(SaveConfigurationRequest request)
        {
            var result = await Mediator.Send(new UpdateConfigurationCommand(request));
            return result ?? DataResponse<int>.Error("Có lỗi khi lưu dữ liệu!");
        }
    }
}
