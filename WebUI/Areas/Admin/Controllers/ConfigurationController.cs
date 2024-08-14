using Application.Configurations.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    public class ConfigurationController : BaseAdminController
    {
        public async Task<IActionResult> Index()
        {
            var result = await Mediator.Send(new GetConfigurationQuery(1));
            return View(result);
        }
    }
}
