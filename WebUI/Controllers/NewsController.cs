using Application.Common.Responses;
using Application.Configurations.Queries;
using Application.Pages.Queries;
using Domain.Constants;
using Microsoft.AspNetCore.Mvc;
using WebUI.Areas.Admin.Controllers;

namespace WebUI.Controllers
{
    public class NewsController : BaseController
    {
        public async Task<IActionResult> Index(int? page)
        {
            ViewBag.MenuParentActive = 3;
            var p = page ?? 1;
            var model = await Mediator.Send(new GetNewsIndexQuery(p));

            var configuration = await Mediator.Send(new GetConfigurationQuery(1));
            ViewBag.Configuration = configuration.Message.Type != MessageType.Success ? null : configuration.Data;

            return View(model);
        }
        public async Task<IActionResult> Project(int? page)
        {
            ViewBag.MenuParentActive = 3;
            ViewBag.MenuChildActive = 31;

            var p = page ?? 1;
            var model = await Mediator.Send(new GetNewsIndexQuery(p, NewsTypeConstant.Project));

            var configuration = await Mediator.Send(new GetConfigurationQuery(1));
            ViewBag.Configuration = configuration.Message.Type != MessageType.Success ? null : configuration.Data;

            return View(model);
        }
        public async Task<IActionResult> Market(int? page)
        {
            ViewBag.MenuParentActive = 3;
            ViewBag.MenuChildActive = 32;

            var p = page ?? 1;
            var model = await Mediator.Send(new GetNewsIndexQuery(p, NewsTypeConstant.Market));

            var configuration = await Mediator.Send(new GetConfigurationQuery(1));
            ViewBag.Configuration = configuration.Message.Type != MessageType.Success ? null : configuration.Data;

            return View(model);
        }
        public async Task<IActionResult> Detail(string? id)
        {
            if (id == null) return Redirect("/PageNotFound");

            var model = await Mediator.Send(new GetNewsDetailQuery(id));

            if (model == null) return Redirect("/PageNotFound");

            ViewBag.MenuParentActive = 3;

            var configuration = await Mediator.Send(new GetConfigurationQuery(1));
            ViewBag.Configuration = configuration.Message.Type != MessageType.Success ? null : configuration.Data;

            return View(model);
        }
    }
}
