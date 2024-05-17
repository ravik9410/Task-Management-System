using AssigneTaskServices.Models;
using AssigneTaskServices.Models.DTO;

namespace AssigneTaskServices.Services.Contract
{
    public interface IAssignTask
    {
        Task<bool> AssignTask(AssignedUserTaskDto assignedUserTask);
        Task<bool> DeleteTask(int assignId);
    }
}
