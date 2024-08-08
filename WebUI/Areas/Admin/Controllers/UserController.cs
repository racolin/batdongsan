using Application.Common.Requests;
using Application.Users.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    public class UserController : BaseAdminController
    {
        public async Task<IActionResult> Index(string? search, string? email, bool? isLock, int? perPage, int? currentPage)
        {
            int current = currentPage ?? 1;
            current = current! < 1 ? 1 : current;

            int length = perPage ?? 8;
            length = length < 1 ? 8 : length;

            var form = new SearchRequest
            {
                Value = search,
                ValueFilter1 = email,
                ValueFilter2 = isLock?.ToString(),
                Start = (current - 1) * length,
                Length = length,
            };

            var result = await Mediator.Send(new GetUsersQuery(form));

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
    }
}
