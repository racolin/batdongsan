using Application.Common.Responses.Views;
using Application.Pages.Queries;
using Domain.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
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
            return View(model);
        }
        public async Task<IActionResult> Project(int? page)
        {
            ViewBag.MenuParentActive = 3;
            ViewBag.MenuChildActive = 31;

            var p = page ?? 1;
            var model = await Mediator.Send(new GetNewsIndexQuery(p, NewsTypeConstant.Project));

            return View(model);
        }
        public async Task<IActionResult> Market(int? page)
        {
            ViewBag.MenuParentActive = 3;
            ViewBag.MenuChildActive = 32;

            var p = page ?? 1;
            var model = await Mediator.Send(new GetNewsIndexQuery(p, NewsTypeConstant.Market));

            return View(model);
        }
        public async Task<IActionResult> Detail(string? id)
        {
            if (id == null) return Redirect("/PageNotFound");

            var model = await Mediator.Send(new GetNewsDetailQuery(id));

            if (model == null) return Redirect("/PageNotFound");

            ViewBag.MenuParentActive = 3;

            return View(model);
        }
    }
}
