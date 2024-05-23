using AssigneTaskServices.Data;
using AssigneTaskServices.Models;
using AssigneTaskServices.Models.DTO;
using AssigneTaskServices.Repository;
using AssigneTaskServices.Services.Contract;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AssigneTaskServices.Services.Implementation
{
    public class AssignTaskService : IAssignTask
    {
        private readonly AppDbContext _appDbContext;
        private readonly IRabbitMQRepository<AssignedUserTask> _rabbitMQRepository;
        private readonly IUserTaskServices _taskServices;
        private readonly IMapper _mapper;
        public AssignTaskService(AppDbContext appDbContext, IMapper mapper, IUserTaskServices taskServices, IRabbitMQRepository<AssignedUserTask> rabbitMQRepository)
        {
            _appDbContext = appDbContext;
            _rabbitMQRepository = rabbitMQRepository;
            _mapper = mapper;
            _taskServices = taskServices;
        }
        public async Task<bool> AssignTask(AssignedUserTaskDto assignedUserTask)
        {
            try
            {
                //check the task is created first.
                var task = await _taskServices.GetTaskById(assignedUserTask.TaskId ?? 0);
                if (task.TaskId > 0)
                {
                    var assignTaskDetails = await _appDbContext.AssignedTasks.AsNoTracking().FirstOrDefaultAsync(m => m.TaskId == task.TaskId && m.UserId == task.UserId);
                    var assign = _mapper.Map<AssignedUserTask>(assignedUserTask);
                    if (assign.AssignedId == 0 && assignTaskDetails == null)
                    {
                        assign.TaskStatus = "Assigned";
                        await _appDbContext.AddAsync(assign);
                        // return await _appDbContext.SaveChangesAsync() == 1 ? true : false;
                    }
                    else
                    {
                        assign.AssignedId = assignTaskDetails.AssignedId;
                        assign.TaskStatus = "Assigned";
                        _appDbContext.Update(assign);

                    }
                    var result = await _appDbContext.SaveChangesAsync() == 1 ? true : false;
                    _rabbitMQRepository.SendAsync(assign);
                    return result;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteTask(int assignId)
        {
            var assignData = await _appDbContext.AssignedTasks.FirstOrDefaultAsync(m => m.TaskId == assignId);
            if (assignData?.AssignedId != 0 && assignData?.AssignedId != null)
            {

                _appDbContext.AssignedTasks.Remove(assignData!);
                var result = await _appDbContext.SaveChangesAsync() == 1 ? true : false;
                assignData.TaskStatus = "Deleted";
                _rabbitMQRepository.SendAsync(assignData);
                return result;
            }
            else
            {
                var task = await _taskServices.GetTaskById(assignId);
                AssignedUserTask assignDto = new AssignedUserTask();
                assignDto.TaskStatus = "Deleted";
                assignDto.TaskId = task.TaskId;
                assignDto.UserId = task.UserId;
                assignDto.TaskAssignedBy = task.TaskAssignedBy;
                //task.TaskStatus = "Deleted";
                _rabbitMQRepository.SendAsync(assignDto);
                return true;
            }
            return false;
        }
    }
}
