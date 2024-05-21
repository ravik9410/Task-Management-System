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
        private readonly ResponseDto _responseDto;

        public AssignTaskController(IAssignTask assignTask)
        {
            _responseDto = new() { IsSuccess = false,Message=string.Empty,Result=string.Empty };
            _assignTask = assignTask;
        }

        [HttpPost]
        public async Task<IActionResult> AssignTask(AssignedUserTaskDto assignModel)
        {
            var data = await _assignTask.AssignTask(assignModel);
            if (data)
            {
                _responseDto.Result = data;
                _responseDto.IsSuccess = true;
                _responseDto.Message = string.Empty;
                return Ok(_responseDto);
            }            
            _responseDto.IsSuccess = false;
            _responseDto.Message = "Error Occured";
            return BadRequest(_responseDto);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateTask(AssignedUserTaskDto assignModel)
        {
            //return await _assignTask.AssignTask(assignModel) == true ? Ok(true) : BadRequest();
            var data = await _assignTask.AssignTask(assignModel);
            if (data)
            {
                _responseDto.Result = data;
                _responseDto.IsSuccess = true;
                _responseDto.Message = string.Empty;
                return Ok(_responseDto);
            }
            _responseDto.IsSuccess = false;
            _responseDto.Message = "Error Occured";
            return BadRequest(_responseDto);
        }
        [HttpDelete("{assignId}")]
        public async Task<IActionResult> DeleteTask(int assignId)
        {
            //return await _assignTask.DeleteTask(assignId) == true ? Ok(true) : BadRequest();
            var data = await _assignTask.DeleteTask(assignId);
            if (data)
            {
                _responseDto.Result = data;
                _responseDto.IsSuccess = true;
                _responseDto.Message = string.Empty;
                return Ok(_responseDto);
            }
            _responseDto.IsSuccess = false;
            _responseDto.Message = "Error Occured";
            return BadRequest(_responseDto);
        }

    }
}
