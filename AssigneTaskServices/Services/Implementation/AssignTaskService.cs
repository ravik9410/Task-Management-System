using AssigneTaskServices.Data;
using AssigneTaskServices.Models;
using AssigneTaskServices.Models.DTO;
using AssigneTaskServices.Services.Contract;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AssigneTaskServices.Services.Implementation
{
    public class AssignTaskService : IAssignTask
    {
        private readonly AppDbContext _appDbContext;
        private readonly IUserTaskServices _taskServices;
        private readonly IMapper _mapper;
        public AssignTaskService(AppDbContext appDbContext, IMapper mapper, IUserTaskServices taskServices)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _taskServices = taskServices;
        }
        public async Task<bool> AssignTask(AssignedUserTaskDto assignedUserTask)
        {
            try
            {
                //check the task is created first.
                var task = await _taskServices.GetTaskById(assignedUserTask.TaskId);
                if (task.TaskId > 0)
                {
                    var assign = _mapper.Map<AssignedUserTask>(assignedUserTask);
                    if (assign.AssignedId == 0)
                    {
                        await _appDbContext.AddAsync(assign);
                        return await _appDbContext.SaveChangesAsync() == 1 ? true : false;
                    }
                    else
                    {
                        _appDbContext.Update(assign);
                        return await _appDbContext.SaveChangesAsync() == 1 ? true : false;
                    }
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
            var assignData = await _appDbContext.AssignedTasks.FirstOrDefaultAsync(m => m.AssignedId == assignId);
            if (assignData?.AssignedId != 0)
            {
                _appDbContext.AssignedTasks.Remove(assignData!);
                return await _appDbContext.SaveChangesAsync() == 1 ? true : false;
            }
            return false;
        }
    }
}
