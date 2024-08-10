using Application.Common.Requests;
using Application.Users.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    public class UserController : BaseAdminController
    {
        public async Task<IActionResult> Index([FromQuery] SearchRequest request)
        {
            request.CurrentPage = request.CurrentPage ?? 1;
            request.PerPage = request.PerPage ?? 8;

            var result = await Mediator.Send(new GetUsersQuery(request));

            return View(result.Data);
        }
        public async Task<IActionResult> Item(int? id)
        {

            if (id == null)
            {
                return View(null);
            }

            var result = await Mediator.Send(new GetUserQuery(id.Value));

            return View(result);
        }
        public async Task<IActionResult> Profile()
        {

            var result = await Mediator.Send(new GetProfileQuery());

            return View(result);
        }
    }
}
