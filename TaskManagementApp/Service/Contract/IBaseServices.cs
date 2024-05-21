using TaskManagementApp.DTO.Models;
using TaskManagementApp.Models.DTO;

namespace TaskManagementApp.Service.Contract
{
    public interface IBaseServices
    {
        Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true);
    }
}
