using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskCreationService.Models.DTO;
using TaskCreationService.Services.Contract;

namespace TaskCreationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskCreation _createTask;
        public TaskController(ITaskCreation createTask)
        {
            _createTask = createTask;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserTaskDto task)
        {
            var result = await _createTask.CreateTask(task);
            if (result == "Task created successfully.")
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
