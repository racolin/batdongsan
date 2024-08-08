using Application.Common.Requests;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.User.Identity?.IsAuthenticated == true)
            {
                return Redirect("/admin/user");
            }
            return View();
        }
        public IActionResult Login()
        {
            if (HttpContext.User.Identity?.IsAuthenticated == true)
            {
                return Redirect("/admin/user");
            }
            return View("~/Areas/Admin/Views/Auth/Index.cshtml");
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        public IActionResult ResetPassword(string token, string username)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(username)) 
            {
                return Redirect("/PageNotFound/Index");
            }  
            return View();
        }
    }
}
