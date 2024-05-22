using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskDashboardServices.Models.DTO;
using TaskDashboardServices.Services.Contract;

namespace TaskDashboardServices.Controllers
{
    [Route("api/taskdashboard")]
    [ApiController]
    [Authorize]
    public class TaskDashboardController : ControllerBase
    {
        private readonly ITaskDashboard _taskDashboard;
        private readonly ResponseDto _responseDto;

        public TaskDashboardController(ITaskDashboard taskDashboard)
        {
            _taskDashboard = taskDashboard;
            _responseDto = new ResponseDto();
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await  _taskDashboard.GetAllTask();
            if (result.Count > 0)
            {
				_responseDto.Result = result;
				_responseDto.IsSuccess = true;
				_responseDto.Message = string.Empty;
				return Ok(_responseDto);  
            }
            else {

				_responseDto.Result = string.Empty;
				_responseDto.IsSuccess = false;
				_responseDto.Message = "No record";
				return BadRequest(_responseDto); }
        }
        [HttpGet("GetTaskbyUserId/{userId}")]
        public async Task<IActionResult> GetTaskByUserId(string userId)
        {
            var result = await _taskDashboard.GetTaskByUserId(userId);
            if (result.Count > 0)
            {
				_responseDto.Result = result;
                _responseDto.IsSuccess = true;
                _responseDto.Message = string.Empty;

				return Ok(_responseDto);
            }
            else {
				_responseDto.Result = string.Empty;
				_responseDto.IsSuccess = false;
				_responseDto.Message = "No Record";
                return BadRequest(_responseDto); }
        }
        [HttpGet("GetTaskListByStatus/{status}")]
        public async Task<IActionResult> GetTaskListByStatus(string status)
        {
            var result = await _taskDashboard.GetTaskListByStatus(status);
            if (result.Count > 0)
            {
				_responseDto.Result = result;
				_responseDto.IsSuccess = true;
				_responseDto.Message = string.Empty;
				return Ok(_responseDto);
            }
            else {
				_responseDto.Result = string.Empty;
				_responseDto.IsSuccess = false;
				_responseDto.Message = "No Record";
				return BadRequest(_responseDto); 
            }
            else { return BadRequest("No record"); }
        }
    }
}
