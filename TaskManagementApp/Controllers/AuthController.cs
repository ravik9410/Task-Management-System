using Microsoft.AspNetCore.Mvc;

namespace TaskManagementApp.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
