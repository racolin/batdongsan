using Microsoft.AspNetCore.Mvc;
using WebUI.Areas.Admin.Controllers;

namespace WebUI.Controllers
{
    public class PageNotFoundController : BaseController
    {
        public IActionResult Index()
        {
            
            return View();
        }
    }
}
