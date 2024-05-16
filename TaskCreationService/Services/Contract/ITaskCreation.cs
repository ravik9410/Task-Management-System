using TaskCreationService.Models.DTO;

namespace TaskCreationService.Services.Contract
{
    public interface ITaskCreation
    {
        Task<string> CreateTask(UserTaskDto task);
    }
}
