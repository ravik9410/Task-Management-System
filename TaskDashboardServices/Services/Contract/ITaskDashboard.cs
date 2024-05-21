using TaskDashboardServices.Models;
using TaskDashboardServices.Models.DTO;

namespace TaskDashboardServices.Services.Contract
{
    public interface ITaskDashboard
    {
        Task<List<UserTaskDto>> GetAllTask();
        Task<List<UserTaskDto>> GetTaskListByStatus(string status);
        Task<List<UserTaskDto>> GetTaskByUserId(string id);
    }
}
