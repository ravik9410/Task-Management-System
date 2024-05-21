using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApp.Models.DTO;
using TaskManagementApp.Service.Contract;

namespace TaskManagementApp.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IManageUser _user;

        public UserController(IManageUser user)
        {
            _user = user;
        }
        [HttpGet]
        public async Task<IActionResult> UserDashboard()
        {
            var data = await _user.GetAllUser();
            if(data.Count==0)
                TempData["error"] = "No records.";
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> EditUser(string userId)
        {
            var data = await _user.GetUserById(userId);
            if (string.IsNullOrEmpty(data.UserId))
                TempData["error"] = "No user with this id.";
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(UserDto userDto)
        {
            var data = await _user.UpdateUser(userDto, userDto.UserId);
            if (string.IsNullOrEmpty(data))
            {
                TempData["success"] = "User updated successfully.";
                return RedirectToAction(nameof(UserDashboard));
            }
            else
            {
                TempData["error"] = data;
                return View(data);
            };
        }
        [HttpGet]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var data = await _user.GetUserById(userId);
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(UserDto userDto)
        {
            var data = await _user.DeleteUser(userDto.UserId);
            TempData["success"] = "User deleted successfully.";
            return RedirectToAction(nameof(UserDashboard));
        }
    }
}
