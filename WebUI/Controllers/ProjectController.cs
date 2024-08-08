using Application.Common.Responses.Views;
using Application.Pages.Queries;
using Application.Projects.Queries;
using Microsoft.AspNetCore.Mvc;
using WebUI.Areas.Admin.Controllers;

namespace WebUI.Controllers
{
    public class ProjectController : BaseController
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.MenuParentActive = 2;

            var model = await Mediator.Send(new GetProjectIndexQuery());
            
            return View(model);
        }
        public async Task<IActionResult> Detail(string? id)
        {
            if (id == null) return Redirect("/PageNotFound");
            ViewBag.MenuParentActive = 2;

            var model = await Mediator.Send(new GetProjectDetailQuery(id));

            if (model == null) return Redirect("/PageNotFound");

            return View(model);
        }
    }
}
