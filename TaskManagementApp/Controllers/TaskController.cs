using Microsoft.AspNetCore.Mvc;

namespace TaskManagementApp.Controllers
{
    public class TaskController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
