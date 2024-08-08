using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    public class DashBoardController : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
