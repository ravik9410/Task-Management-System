using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagementApp.Models.DTO;
using TaskManagementApp.Service.Contract;
using static TaskManagementApp.Utility.StaticData;

namespace TaskManagementApp.Service.Implementation
{
    public class TaskManagement : ITaskManagement
    {
        private readonly IBaseServices _baseService;
        private readonly ITokenHandler _tokenHandler;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IManageUser _userManage;
        public TaskManagement(IBaseServices baseService, ITokenHandler tokenHandler, IHttpContextAccessor contextAccessor, IManageUser userManage)
        {
            _baseService = baseService;
            _tokenHandler = tokenHandler;
            _contextAccessor = contextAccessor;
            this._userManage = userManage;
        }

        public async Task<string> AssignedTask(UserTaskDto userTaskDto)
        {
            userTaskDto.TaskAssignedBy = _contextAccessor.HttpContext!.User.Claims.FirstOrDefault(m => m.Type == JwtRegisteredClaimNames.Sub)?.Value;
            AssignedUserTaskDto assignedTask = new AssignedUserTaskDto() { TaskAssignedBy = userTaskDto.TaskAssignedBy, TaskId = userTaskDto.TaskId, TaskStatus = userTaskDto.TaskStatus, UserId = userTaskDto.UserId };
            var results = await _baseService.SendAsync(new()
            {
                ApiType = ApiType.POST,
                Url = TaskDashboardUrl + "/api/task/"+userTaskDto.TaskId,
                //Data = assignedTask
            });
            if (results!.IsSuccess)
            {
                var data = JsonConvert.DeserializeObject<UserTaskDto>(results.Result?.ToString()!);
                //assignedTask.
                //return data!;
            }

           
            var result = await _baseService.SendAsync(new()
            {
                ApiType = ApiType.POST,
                Url = TaskDashboardUrl + "/api/assigntask",
                Data = assignedTask
            });
            if (result!.IsSuccess)
            {
                
                return result.Result?.ToString()!;
            }
            return string.Empty;
        }

        public async Task<string> CreateTask(UserTaskDto userTaskDto)
        {
            userTaskDto.TaskAssignedBy = _contextAccessor.HttpContext!.User.Claims.FirstOrDefault(m => m.Type == JwtRegisteredClaimNames.Sub)?.Value;
            var result = await _baseService.SendAsync(new()
            {
                ApiType = ApiType.POST,
                Url = TaskDashboardUrl + "/api/task",
                Data = userTaskDto
            });
            if (result!.IsSuccess)
            {
                var data = (result.Result?.ToString()!);
                return data!;
            }
            return result.Message!;
        }

        public async Task<string> DeleteTask(UserTaskDto userTaskDto)
        {
            var result = await _baseService.SendAsync(new()
            {
                ApiType = ApiType.DELETE,
                Url = TaskDashboardUrl + "/api/assigntask/"+userTaskDto.TaskId,
            });
            if (result!.IsSuccess)
            {
                var data = result.Result?.ToString()!;                
                return data!;
            }
            return false.ToString();
        }

        public async Task<List<UserTaskDto>> GetAllTask()
        {
            var result = await _baseService.SendAsync(new()
            {
                ApiType = ApiType.GET,
                Url = TaskDashboardUrl + "/api/taskdashboard",
            });
            if (result!.IsSuccess)
            {
                var data = JsonConvert.DeserializeObject<List<UserTaskDto>>(result.Result?.ToString()!);
                foreach (var item in data!)
                {
                    if (!string.IsNullOrEmpty(item.UserId))
                    {
                        var userDetails = await _userManage.GetUserById(item.UserId);
                        item.UserName = userDetails.Name ?? string.Empty;
                    }
                    else
                    {
                        item.UserName = "N/A";
                    }
                    if (!string.IsNullOrEmpty(item.TaskAssignedBy))
                    {
                        var assignedDetails = await _userManage.GetUserById(item.TaskAssignedBy);
                        item.AssignedByName = assignedDetails.Name ?? string.Empty;
                    }
                    else
                    {
                        item.AssignedByName = "N/A";
                    }
                }
                return data!;
            }
            return new();
        }

        public async Task<List<UserDto>> GetAllUser()
        {
            var result = await _userManage.GetAllUser();
            return result;
        }

        public async Task<UserTaskDto> GetTaskById(int taskId)
        {
            var result = await _baseService.SendAsync(new()
            {
                ApiType = ApiType.GET,
                Url = TaskDashboardUrl + "/api/task/" + taskId,
                // Data = userTaskDto
            });
            if (result!.IsSuccess)
            {
                var data = JsonConvert.DeserializeObject<UserTaskDto>(result.Result?.ToString()!);
                if (!string.IsNullOrEmpty(data.UserId))
                {
                    var userDetails = await _userManage.GetUserById(data.UserId);
                    data.UserName = userDetails.Name ?? string.Empty;
                }
                else
                {
                    data.UserName = "N/A";
                }
                return data!;
            }
            return new();
        }

        public async Task<string> UpdateTask(UserTaskDto userTaskDto)
        {
            userTaskDto.TaskAssignedBy = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(m => m.Type == JwtRegisteredClaimNames.Sub).Value;
            var result = await _baseService.SendAsync(new()
            {
                ApiType = ApiType.PATCH,
                Url = TaskDashboardUrl + "/api/task",
                Data = userTaskDto
            });
            if (result!.IsSuccess)
            {
                var data = result.Result?.ToString()!;
                return data!;
            }
            return string.Empty;
        }

        public async Task<List<UserTaskDto>> UserTaskById(string userId)
        {
            var result = await _baseService.SendAsync(new()
            {
                ApiType = ApiType.GET,
                Url = TaskDashboardUrl + "/api/taskdashboard/GetTaskbyUserId/" + userId,
            });
            if (result!.IsSuccess)
            {
                var data = JsonConvert.DeserializeObject<List<UserTaskDto>>(result.Result?.ToString()!);
                foreach (var item in data!)
                {
                    var userDetails = await _userManage.GetUserById(item.UserId);
                    item.UserName = userDetails.Name ?? string.Empty;
                    var assignedDetails = await _userManage.GetUserById(item.TaskAssignedBy);
                    item.AssignedByName = assignedDetails.Name ?? string.Empty;
                }
                return data!;
            }
            return new();
        }
    }
}
