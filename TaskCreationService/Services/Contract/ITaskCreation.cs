using TaskCreationService.Models.DTO;

namespace TaskCreationService.Services.Contract
{
    public interface ITaskCreation
    {
        Task<string> CreateTask(UserTaskDto task);
        Task<List<UserTaskDto>> GetTask();
        Task<UserTaskDto> GetTaskById(int taskId);
        List<UserTaskDto> GetTaskByUserId(string userId);
    }
}
