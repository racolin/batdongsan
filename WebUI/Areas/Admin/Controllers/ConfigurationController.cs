using Application.Configurations.Queries;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = $"{RoleConstant.Admin}")]
    public class ConfigurationController : BaseAdminController
    {
        public async Task<IActionResult> Index()
        {
            var result = await Mediator.Send(new GetConfigurationQuery(1));
            return View(result);
        }
    }
}
