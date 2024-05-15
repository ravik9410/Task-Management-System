using UserManagementService.Models.DTO;

namespace UserManagementService.Services.Contract
{
    public interface IUserManage
    {
        /*Task<int> CreateUserAsync();*/
        Task<int> UpdateUserAsync(string userId,UserDto users);
        Task<List<UserDto>> GetUsersAsync();
        Task<UserDto> GetUserByIdAsync(string userId);
        Task<int> DeleteUserAsync(string userId);
    }
}
