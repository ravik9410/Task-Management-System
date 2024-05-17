using AssigneTaskServices.Models.DTO;

namespace AssigneTaskServices.Services.Contract
{
    public interface IUserTaskServices
    {
        Task<UserTaskDto> GetTaskById(int taskId);
    }
}
