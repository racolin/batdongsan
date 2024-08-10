using Application.Common.Requests;
using Application.Contacts.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    public class ContactController : BaseAdminController
    {
        public async Task<IActionResult> Index([FromQuery] SearchRequest request)
        {
            request.CurrentPage = request.CurrentPage ?? 1;
            request.PerPage = request.PerPage ?? 12;
            request.Order = request.Order ?? "desc-date";

            var result = await Mediator.Send(new GetContactsQuery(request));

            return View(result.Data);
        }
    }
}
