using AssigneTaskServices.Migrations;
using AssigneTaskServices.Models.DTO;
using AssigneTaskServices.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AssigneTaskServices.Controllers
{
    [Route("api/assigntask")]
    [ApiController]
    [Authorize]
    public class AssignTaskController : ControllerBase
    {
        private readonly IAssignTask _assignTask;

        public AssignTaskController(IAssignTask assignTask)
        {
            _assignTask = assignTask;
        }

        [HttpPost]
        public async Task<IActionResult> AssignTask(AssignedUserTaskDto assignModel)
        {
            return await _assignTask.AssignTask(assignModel) == true ? Ok(true) : BadRequest();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateTask(AssignedUserTaskDto assignModel)
        {
            return await _assignTask.AssignTask(assignModel) == true ? Ok(true) : BadRequest();
        }
        [HttpDelete("{assignId}")]
        public async Task<IActionResult> DeleteTask(int assignId)
        {
            return await _assignTask.DeleteTask(assignId) == true ? Ok(true) : BadRequest();
        }

    }
}
