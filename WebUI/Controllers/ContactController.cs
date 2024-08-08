using Application.Pages.Queries;
using Microsoft.AspNetCore.Mvc;
using WebUI.Areas.Admin.Controllers;

namespace WebUI.Controllers
{
    public class ContactController : BaseController
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.MenuParentActive = 4;

            var model = await Mediator.Send(new GetContactIndexQuery());

            return View(model!);
        }
    }
}
