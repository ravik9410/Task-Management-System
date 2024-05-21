using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskCreationService.Models.DTO;
using TaskCreationService.Services.Contract;

namespace TaskCreationService.Controllers
{
    [Route("api/task")]
    [ApiController]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskCreation _createTask;
        ResponseDto _responseDto;
        public TaskController(ITaskCreation createTask)
        {
            _createTask = createTask;
            _responseDto = new ResponseDto();
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserTaskDto task)
        {
            var result = await _createTask.CreateTask(task);
            if (result == "Task created successfully.")
            {
                _responseDto.IsSuccess = true;
                _responseDto.Result = result;
                _responseDto.Message = string.Empty;
                return Ok(_responseDto);
            }
            _responseDto.IsSuccess = false;
            _responseDto.Message = result;
            _responseDto.Result = string.Empty;
            return BadRequest(result);
        }
        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody] UserTaskDto task)
        {
            var result = await _createTask.UpdateTask(task);
            if (result == "Task saved successfully.")
            {
                _responseDto.IsSuccess = true;
                _responseDto.Result = result;
                _responseDto.Message = string.Empty;
                return Ok(_responseDto);
            }
            _responseDto.IsSuccess = false;
            _responseDto.Message = result;
            _responseDto.Result = string.Empty;
            return BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetTaskList()
        {
            var result = await _createTask.GetTask();
            if (result.Count > 0)
            {
                _responseDto.IsSuccess = true;
                _responseDto.Result = result;
                _responseDto.Message = string.Empty;
                return Ok(_responseDto);
            }
            _responseDto.IsSuccess = false;
            _responseDto.Message = "no record found";
            _responseDto.Result = string.Empty;
            return BadRequest(_responseDto);
        }
        [HttpGet("{taskId:int}")]
        public async Task<IActionResult> GetTaskById(int taskId)
        {
            var result = await _createTask.GetTaskById(taskId);
            if (result.TaskId != 0)
            {
                _responseDto.IsSuccess = true;
                _responseDto.Result = result;
                _responseDto.Message = string.Empty;
                return Ok(result);
            }
            _responseDto.IsSuccess = false;
            _responseDto.Message = "no record ";
            _responseDto.Result = string.Empty;
            return BadRequest(result);
        }

        [HttpGet("GetTaskByUserId/{userId}")]
        public IActionResult GetTaskByUserId(string userId)
        {
            var result = _createTask.GetTaskByUserId(userId);
            if (result.Count > 0)
            {
                _responseDto.IsSuccess = true;
                _responseDto.Result = result;
                _responseDto.Message = string.Empty;
                return Ok(result);
            }
            _responseDto.IsSuccess = true;
            _responseDto.Message = "no record";
            _responseDto.Result = string.Empty;
            return BadRequest(result);
        }   
        [HttpGet("GetTaskByStatus/{status}")]
        public IActionResult GetTaskByStatusId(string status)
        {
            var result = _createTask.GetTaskByStatus(status);
            if (result.Count > 0)
            {
                _responseDto.IsSuccess = true;
                _responseDto.Result = result;
                _responseDto.Message = string.Empty;
                return Ok(result);
            }
            _responseDto.IsSuccess = true;
            _responseDto.Message = "no record";
            _responseDto.Result = string.Empty;
            return BadRequest(result);
        }

    }
}
