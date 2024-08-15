using Application.Common.Requests;
using Application.Common.Responses;
using Application.News.Commands;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers.Apis
{
    [ApiExplorerSettings(GroupName = "News - Admin")]
    public class NewsController : ApiAdminControllerBase
    {
        [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.NewsPoster}")]
        [HttpPost("save")]
        public async Task<DataResponse<int>> Save(SaveNewsRequest request)
        {
            var result = await Mediator.Send(new SaveNewsCommand(request));
            return result ?? DataResponse<int>.Error("Có lỗi khi lưu dữ liệu!");
        }
    }
}
