using Application.Common.Requests;
using Application.Common.Responses;
using Application.Common.Supports;
using Application.Projects.Queries;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = $"{RoleConstant.Admin},{RoleConstant.ProjectPoster}")]
    public class ProjectController : BaseAdminController
    {
        public async Task<IActionResult> Index([FromQuery] SearchRequest request)
        {
            request.CurrentPage = request.CurrentPage ?? 1;
            request.PerPage = request.PerPage ?? 8;
            request.Order = request.Order ?? "desc-order";

            var result = await Mediator.Send(new GetProjectsQuery(request));

            return View(result.Data);
        }
        public async Task<IActionResult> Item(int? id)
        {

            if (id == null)
            {
                return View(null);
            }

            var result = await Mediator.Send(new GetProjectQuery(id.Value));

            if (result.Message.Type != MessageType.Error)
            {
                if (result.Data.Image?.Name == null)
                {
                    ViewBag.Src = DefaultConstant.NoImage;
                }
                else
                {
                    ViewBag.Src = PathSupport.GetUploadThumbDefaultPath(result.Data.Image.Name, result.Data.Image.Type);
                }
            }

            return View(result);
        }
    }
}
