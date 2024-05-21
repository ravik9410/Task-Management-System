using TaskManagementApp.Models.DTO;
using TaskManagementApp.Service.Contract;

namespace TaskManagementApp.Service.Implementation
{
    public class AuthenticationService : IAuthentication
    {
        public Task<UserDto> Login(LoginRequestDto login)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> Registration(RegisterationRequestDto register)
        {
            throw new NotImplementedException();
        }
    }
}
