using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    [Authorize]
    public class PageNotFoundController : BaseAdminController
    { 
        public IActionResult Index()
        {
            return View();
        }
    }
}
