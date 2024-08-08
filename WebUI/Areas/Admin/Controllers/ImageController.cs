using Application.Common.Requests;
using Application.Images.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    public class ImageController : BaseAdminController
    {
        public async Task<IActionResult> Index(string? search, int? perPage, int? currentPage, string? order, string? startDate, string? endDate)
        {
            int current = currentPage ?? 1;
            current = current! < 1 ? 1 : current;

            int length = perPage ?? 12;
            length = length < 1 ? 12 : length;

            DateTime? start = null;
            if (startDate != null)
            {
                try
                {
                    start = DateTime.ParseExact(startDate, "dd/MM/yyyy", null);
                }
                catch (Exception ex) {
                
                }
            }

            DateTime? end = null;
            if (endDate != null)
            {
                try
                {
                    end = DateTime.ParseExact(endDate, "dd/MM/yyyy", null);
                    end = end.Value.AddDays(1);
                }
                catch (Exception ex) {
                
                }
            }

            List<string> ls = order?.Split("-").ToList() ?? new List<string>();

            var form = new SearchRequest
            {
                Value = search,
                Start = (current - 1) * length,
                Length = length,
                Order = ls.Count > 1 ? ls[1] : "date",
                IsAsc = ls.Count > 1 ? ls[0].Equals("asc") : true,
                EndDate = end,
                StartDate = start,
            };

            var result = await Mediator.Send(new GetImagesQuery(form));

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
