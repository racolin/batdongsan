using Application.Common.Requests;
using Application.Common.Supports;
using Application.News.Queries;
using Domain.Constants;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    public class NewsController : BaseAdminController
    {
        public async Task<IActionResult> Index(string? search, string? status, string? highLight, string? type, int? perPage, int? currentPage, string? order, string? startDate, string? endDate)
        {
            int current = currentPage ?? 1;
            current = current! < 1 ? 1 : current;

            int length = perPage ?? 8;
            length = length < 1 ? 8 : length;

            DateTime? start = null;
            if (startDate != null)
            {
                try
                {
                    start = DateTime.ParseExact(startDate, "dd/MM/yyyy", null);
                }
                catch (Exception ex)
                {

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
                catch (Exception ex)
                {

                }
            }

            List<string> ls = order?.Split("-").ToList() ?? new List<string>();

            var form = new SearchRequest
            {
                Value = search,
                Start = (current - 1) * length,
                Length = length,
                Order = ls.Count > 1 ? ls[1] : "order",
                IsAsc = ls.Count > 1 ? ls[0].Equals("asc") : false,
                EndDate = end,
                StartDate = start,
                Status = status?.Split(",").ToList() ?? StatusConstant.GetAllProperties(),
                ValueFilter1 = highLight,
                ValuesFilter1 = type?.Split(",").ToList() ?? NewsTypeConstant.GetAllProperties(),
            };

            var result = await Mediator.Send(new GetNewsListQuery(form));

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
