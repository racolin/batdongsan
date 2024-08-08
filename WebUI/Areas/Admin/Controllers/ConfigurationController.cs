using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    public class ConfigurationController : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
