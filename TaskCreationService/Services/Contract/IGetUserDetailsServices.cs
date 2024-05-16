using TaskCreationService.Models.DTO;

namespace TaskCreationService.Services.Contract
{
    public interface IGetUserDetailsServices
    {
        Task<UserDto> GetById(string userId);
    }
}
