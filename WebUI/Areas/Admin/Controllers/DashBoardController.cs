using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = $"{RoleConstant.Admin}")]
    public class DashBoardController : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
