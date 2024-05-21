using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskDashboardServices.Services.Contract;

namespace TaskDashboardServices.Controllers
{
    [Route("api/taskdashboard")]
    [ApiController]
    [Authorize]
    public class TaskDashboardController : ControllerBase
    {
        private readonly ITaskDashboard _taskDashboard;

        public TaskDashboardController(ITaskDashboard taskDashboard)
        {
            _taskDashboard = taskDashboard;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await  _taskDashboard.GetAllTask();
            if (result.Count > 0)
            {
                return Ok(result);  
            }
            else { return BadRequest("No record"); }
        }
        [HttpGet("GetTaskbyUserId/{userId}")]
        public async Task<IActionResult> GetTaskByUserId(string userId)
        {
            var result = await _taskDashboard.GetTaskByUserId(userId);
            if (result.Count > 0)
            {
                return Ok(result);
            }
            else { return BadRequest("No record"); }
        }
        [HttpGet("GetTaskListByStatus/{status}")]
        public async Task<IActionResult> GetTaskListByStatus(string status)
        {
            var result = await _taskDashboard.GetTaskListByStatus(status);
            if (result.Count > 0)
            {
                return Ok(result);
            }
            else { return BadRequest("No record"); }
        }
    }
}
