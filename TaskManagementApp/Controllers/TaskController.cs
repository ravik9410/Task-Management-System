using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManagementApp.Models.DTO;
using TaskManagementApp.Service.Contract;
using TaskManagementApp.Utility;

namespace TaskManagementApp.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ITaskManagement _taskManagement;

        public TaskController(ITaskManagement taskManagement)
        {
            _taskManagement = taskManagement;
        }

        [HttpGet]
        public async Task<IActionResult> GetTaskByUserId(string userId)
        {
            var data = await _taskManagement.UserTaskById(userId);
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> TaskDashboard()
        {
            var data = await _taskManagement.GetAllTask();
            if (data.Count == 0)
                TempData["error"] = "No records.";
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> CreateNewTask()
        {
            var userList = await _taskManagement.GetAllUser();
            var roleList = new List<SelectListItem>();
            foreach (var user in userList)
            {
                roleList.Add(new() { Text = user.Name, Value = user.UserId });
            }
            ViewBag.UserList = roleList;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateNewTask(UserTaskDto userDto)
        {
            var result = await _taskManagement.CreateTask(userDto);
            if (result == "Task created successfully.")
            {
                return RedirectToAction(nameof(TaskDashboard));
            }
            TempData["error"] = result;
            return View(userDto);
        }
        [HttpGet]
        public async Task<IActionResult> EditTask(int taskId)
        {
            var userList = await _taskManagement.GetTaskById(taskId);
            return View(userList);
        }
        [HttpPost]
        public async Task<IActionResult> EditTask(UserTaskDto userDto)
        {
            var result = await _taskManagement.UpdateTask(userDto);
            if (result == "Task saved successfully.")
            {
                return RedirectToAction(nameof(TaskDashboard));
            }
            TempData["error"] = result;
            return View(userDto);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            var userList = await _taskManagement.GetTaskById(taskId);
            return View(userList);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteTask(UserTaskDto userDto)
        {
            var result = await _taskManagement.DeleteTask(userDto);
            if (result.ToLower() == "true")
            {
                return RedirectToAction(nameof(TaskDashboard));
            }
            TempData["error"] = result;
            return View(userDto);
        }
        [HttpGet]
        public async Task<IActionResult> GetTaskById(int taskId)
        {
            var result = await _taskManagement.GetTaskById(taskId);
            if (result.TaskId != 0)
            {
                var userList = await _taskManagement.GetAllUser();
                var roleList = new List<SelectListItem>();
                foreach (var user in userList)
                {
                    roleList.Add(new() { Text = user.Name, Value = user.UserId });
                }
                ViewBag.UserList = roleList;
                return View(result);
            }
            TempData["error"] = result;
            return View(taskId);
        }
        [HttpPost]
        public async Task<IActionResult> AssignedTasktoUser(UserTaskDto userDto)
        {
            var result = await _taskManagement.AssignedTask(userDto);
            if (result.ToLower() == "true")
            {
                return RedirectToAction(nameof(TaskDashboard));
            }
            TempData["error"] = result;
            return View(userDto);
        }
    }
}
