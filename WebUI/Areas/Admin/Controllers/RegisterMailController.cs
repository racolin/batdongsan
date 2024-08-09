using Application.Common.Requests;
using Application.RegisterMails.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    public class RegisterMailController : BaseAdminController
    {
        public async Task<IActionResult> Index([FromQuery] SearchRequest request)
        {
            request.CurrentPage = request.CurrentPage ?? 1;
            request.PerPage = request.PerPage ?? 12;

            var result = await Mediator.Send(new GetRegisterMailsQuery(request));

            return View(result.Data);
        }

    }
}
