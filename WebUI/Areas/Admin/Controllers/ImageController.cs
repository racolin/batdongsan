using Application.Common.Requests;
using Application.Images.Queries;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.ImagePoster}")]
    public class ImageController : BaseAdminController
    {
        public async Task<IActionResult> Index([FromQuery] SearchRequest request)
        {
            request.CurrentPage = request.CurrentPage ?? 1;
            request.PerPage = request.PerPage ?? 12;
            request.Order = request.Order ?? "desc-date";

            var result = await Mediator.Send(new GetImagesQuery(request));

            return View(result.Data);
        }
        public async Task<IActionResult> Item(int? id)
        {
            if (id == null)
            {
                return View(null);
            } 

            var result = await Mediator.Send(new GetImageQuery(id.Value));

            return View(result);
        }
    }
}
