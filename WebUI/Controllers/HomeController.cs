using Application.Common.Responses;
using Application.Configurations.Queries;
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

            var configuration = await Mediator.Send(new GetConfigurationQuery(1));
            ViewBag.Configuration = configuration.Message.Type != MessageType.Success ? null : configuration.Data;

            return View(model!);
        }
    }
}
