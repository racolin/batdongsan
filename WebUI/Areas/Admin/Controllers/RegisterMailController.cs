using Application.Common.Requests;
using Application.RegisterMails.Queries;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.RegisteredEmailChecker}")]
    public class RegisterMailController : BaseAdminController
    {
        public async Task<IActionResult> Index([FromQuery] SearchRequest request)
        {
            request.CurrentPage = request.CurrentPage ?? 1;
            request.PerPage = request.PerPage ?? 12;
            request.Order = request.Order ?? "desc-date";

            var result = await Mediator.Send(new GetRegisterMailsQuery(request));

            return View(result.Data);
        }

    }
}
