using AssigneTaskServices.Models;

namespace NotificationServices.Services
{
    public interface ITaskUpdation
    {
        Task<bool> ModifyTaskStatus(AssignedUserTask task);
        Task<string> GetUserDetailsById(string userId);
    }
}
