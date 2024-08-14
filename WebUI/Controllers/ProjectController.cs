using Application.Common.Responses;
using Application.Common.Responses.Views;
using Application.Configurations.Queries;
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

            var configuration = await Mediator.Send(new GetConfigurationQuery(1));
            ViewBag.Configuration = configuration.Message.Type != MessageType.Success ? null : configuration.Data;

            return View(model);
        }
        public async Task<IActionResult> Detail(string? id)
        {
            if (id == null) return Redirect("/PageNotFound");
            ViewBag.MenuParentActive = 2;

            var model = await Mediator.Send(new GetProjectDetailQuery(id));

            var configuration = await Mediator.Send(new GetConfigurationQuery(1));
            ViewBag.Configuration = configuration.Message.Type != MessageType.Success ? null : configuration.Data;

            if (model == null) return Redirect("/PageNotFound");

            return View(model);
        }
    }
}
