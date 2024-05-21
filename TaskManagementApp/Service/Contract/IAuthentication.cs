using TaskManagementApp.Models.DTO;

namespace TaskManagementApp.Service.Contract
{
    public interface IAuthentication
    {
       Task<UserDto> Login(LoginRequestDto login);
       Task Logout();
       Task<string> Registration(RegisterationRequestDto register);
    }
}
