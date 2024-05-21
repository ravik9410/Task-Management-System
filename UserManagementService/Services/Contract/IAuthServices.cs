using UserManagementService.Models.DTO;

namespace UserManagementService.Services.Contract
{
    public interface IAuthServices
    {
        Task<LoginResponseDto> Login(LoginRequestDto loginRequest);
        Task<string> Register(RegisterationRequestDto registerationRequest);
        Task<bool> AssignRole(string email, string role);
        Task<UserDto> GetUserByIdAsync(string userId);
    }
}
