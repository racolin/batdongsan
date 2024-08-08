using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    public class PageNotFoundController : BaseAdminController
    { 
        public IActionResult Index()
        {
            return View();
        }
    }
}
