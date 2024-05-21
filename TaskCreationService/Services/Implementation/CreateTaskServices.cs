using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskCreationService.Data;
using TaskCreationService.Models;
using TaskCreationService.Models.DTO;
using TaskCreationService.Services.Contract;

namespace TaskCreationService.Services.Implementation
{
    public class CreateTaskServices : ITaskCreation
    {
        private readonly AppDbContext _appDbContext;
        private readonly IGetUserDetailsServices _userDetails;
        private readonly IMapper _mapper;

        public CreateTaskServices(AppDbContext appDbContext, IMapper mapper, IGetUserDetailsServices userDetails)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _userDetails = userDetails;
        }

        public async Task<string> CreateTask(UserTaskDto task)
        {
            var user = await _userDetails.GetById(task.UserId);
            if (user.UserId != null)
            {
                var tasks = _mapper.Map<UserTask>(task);
                await _appDbContext.UserTasks.AddAsync(tasks);
                await _appDbContext.SaveChangesAsync();
                return "Task created successfully.";
            }
            else
            {
                return "User not exist.";
            }
        }
        public async Task<string> UpdateTask(UserTaskDto task)
        {
            var data = await _appDbContext.UserTasks.FirstOrDefaultAsync(m => m.TaskId == task.TaskId);
            //var data = await _userDetails.GetById(task.UserId);
            if (data!.TaskId != 0)
            {
                data.TaskStatus = task.TaskStatus;
                data.TaskAssignedBy = task.TaskAssignedBy;
                await _appDbContext.SaveChangesAsync();
                return "Task saved successfully.";
            }
            else
            {
                return "Task not exist.";
            }
        }

        public async Task<List<UserTaskDto>> GetTask()
        {
            var data = await _appDbContext.UserTasks.ToListAsync();
            var result = _mapper.Map<List<UserTaskDto>>(data);
            return result;
        }

        public async Task<UserTaskDto> GetTaskById(int taskId)
        {
            var data = await _appDbContext.UserTasks.FirstOrDefaultAsync(m => m.TaskId == taskId);
            var result = _mapper.Map<UserTaskDto>(data);
            return result;
            //throw new NotImplementedException();
        }
        public List<UserTaskDto> GetTaskByUserId(string userId)
        {
            var data = _appDbContext.UserTasks.Where(m => m.UserId == userId).ToList();
            var result = _mapper.Map<List<UserTaskDto>>(data);
            return result;
            //throw new NotImplementedException();
        }
        public List<UserTaskDto> GetTaskByStatus(string status)
        {
            var data = _appDbContext.UserTasks.Where(m => m.TaskStatus == status).ToList();
            var result = _mapper.Map<List<UserTaskDto>>(data);
            return result;
            //throw new NotImplementedException();
        }
    }
}
