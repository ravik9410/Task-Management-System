using TaskManagementApp.Models.DTO;

namespace TaskManagementApp.Service.Contract
{
	public interface ITaskManagement
	{
		Task<List<UserTaskDto>> UserTaskById(string userId);
		Task<List<UserTaskDto>> GetAllTask();
		Task<UserTaskDto> GetTaskById( int taskId);
		Task<List<UserDto>> GetAllUser();
		Task<string> CreateTask(UserTaskDto userTaskDto);
		Task<string> AssignedTask(UserTaskDto userTaskDto);
		Task<string> UpdateTask(UserTaskDto userTaskDto);
		Task<string> DeleteTask(UserTaskDto userTaskDto);

	}
}
