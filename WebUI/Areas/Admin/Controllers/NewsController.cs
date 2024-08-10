using Application.Common.Requests;
using Application.Common.Supports;
using Application.News.Queries;
using Domain.Constants;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    public class NewsController : BaseAdminController
    {
        public async Task<IActionResult> Index([FromQuery] SearchRequest request)
        {
            request.CurrentPage = request.CurrentPage ?? 1;
            request.PerPage = request.PerPage ?? 12;
            request.Order = request.Order ?? "desc-order";

            var result = await Mediator.Send(new GetNewsListQuery(request));

            return View(result.Data);
        }
        public async Task<IActionResult> Item(int? id)  
        {

            if (id == null)
            {
                return View(null);
            }

            var result = await Mediator.Send(new GetNewsQuery(id.Value));

            if (result?.Data?.Image?.Name == null)
            {

                ViewBag.Src = DefaultConstant.NoImage;
            } else
            {
                ViewBag.Src =  PathSupport.GetUploadThumbDefaultPath(result.Data.Image.Name, result.Data.Image.Type);
            }

            return View(result);
        }
    }
}
