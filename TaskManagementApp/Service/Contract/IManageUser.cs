using TaskManagementApp.Models.DTO;

namespace TaskManagementApp.Service.Contract
{
	public interface IManageUser
	{
		Task<List<UserDto>> GetAllUser();
		Task<UserDto> GetUserById(string userId);
		Task<string> UpdateUser(UserDto user,string userId);
		Task<string> DeleteUser(string userId);
		
	}
}
