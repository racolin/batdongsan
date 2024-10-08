﻿using Application.Common.Requests;
using Application.Contents.Queries;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = $"{RoleConstant.Admin}")]
    public class ContentController : BaseAdminController
    {
        public async Task<IActionResult> Index([FromQuery] SearchRequest request)
        {
            request.CurrentPage = request.CurrentPage ?? 1;
            request.PerPage = request.PerPage ?? 12;
            request.Order = request.Order ?? "desc-date";

            var home = await Mediator.Send(new GetContentsQuery(request));

            return View(home.Data);
        }
        public async Task<IActionResult> Item(int? id)
        {
            if (id == null)
            {
                return View(null);
            }

            var result = await Mediator.Send(new GetContentQuery(id.Value));

            return View(result);
        }
    }
}
