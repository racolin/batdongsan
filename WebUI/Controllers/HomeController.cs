using Application.Common.Responses.Views;
using Application.Pages.Queries;
using Microsoft.AspNetCore.Mvc;
using WebUI.Areas.Admin.Controllers;

namespace WebUI.Controllers
{
    public class HomeController : BaseController
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.MenuParentActive = 1;

            var model = await Mediator.Send(new GetHomeIndexQuery());
            
            return View(model!);
        }
    }
}
