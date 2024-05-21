using Microsoft.AspNetCore.Mvc;

namespace TaskManagementApp.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
