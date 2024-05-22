using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManagementApp.Models.DTO;
using TaskManagementApp.Service.Contract;
using TaskManagementApp.Utility;

namespace TaskManagementApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthentication _authenticationService;

        public AuthController(IAuthentication authenticationService)
        {
            _authenticationService = authenticationService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto login)
        {
            var result = await _authenticationService.Login(login);
            if (!string.IsNullOrEmpty(result.UserId))
            {
                return RedirectToAction("Index", "Home");
            }
            return View(login);
        }
        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
        {
                new (){Text=StaticData.RoleAdmin,Value=StaticData.RoleAdmin},
                new (){Text=StaticData.RoleCustomer,Value=StaticData.RoleCustomer},
            };
            ViewBag.RoleList = roleList;
           // ViewBag.RoleList = "";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterationRequestDto register)
        {
            var result = await _authenticationService.Registration(register);
            if (string.IsNullOrEmpty(result))
            {
                return RedirectToAction("Login", "Auth");
            }
            TempData["error"]=result;
            return View(register);
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _authenticationService.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}
