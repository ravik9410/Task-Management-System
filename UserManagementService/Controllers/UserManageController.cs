using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagementService.Models.DTO;
using UserManagementService.Services.Contract;

namespace UserManagementService.Controllers
{
    [Authorize]
    [Route("api/usermanagement")]
    [ApiController]
    public class UserManageController : ControllerBase
    {
        private readonly IUserManage _userServices;
        private ResponseDto _responseDto;

        public UserManageController(IUserManage userServices)
        {
            _userServices = userServices;
            _responseDto = new ResponseDto();
        }
        [HttpGet]
        public async Task<ResponseDto> GetAll()
        {
            var user = await _userServices.GetUsersAsync();
            if (user.Count > 0)
            {
                return new()
                {
                    Result = user,
                    IsSuccess = true,
                    Message = string.Empty
                };
            }
            else
            {
                return new()
                {
                    Result = "No Record",
                    Message = "No Record",
                    IsSuccess = false
                };
            }
        }
        [HttpGet("{userId}")]
        public async Task<ResponseDto> GetById(string userId)
        {
            var user = await _userServices.GetUserByIdAsync(userId);
            if (user.UserId != null)
            {
                return new()
                {
                    Result = user,
                    IsSuccess = true,
                    Message = string.Empty
                };
            }
            else
            {
                return new()
                {
                    Result = "No user found with id = " + userId,
                    Message = "No user found with id = " + userId,
                    IsSuccess = false
                };
            }
        }
        [HttpDelete("{userId}")]
        public async Task<ResponseDto> DeleteById(string userId)
        {
            var user = await _userServices.DeleteUserAsync(userId);
            if (user == 1)
            {
                return new()
                {
                    Result = user,
                    IsSuccess = true,
                    Message = string.Empty
                };
            }
            else
            {
                return new()
                {
                    Result = "No user found with id = " + userId,
                    Message = "No user found with id = " + userId,
                    IsSuccess = false
                };
            }
        }
        [HttpPut("{userId}")]
        public async Task<ResponseDto> Update(string userId,UserDto users)
        {
            if(userId!=users.UserId)
            {
                return new()
                {
                    Result = "user id is not matched.",
                    IsSuccess = false,
                    Message = string.Empty
                };
            }
            var user = await _userServices.UpdateUserAsync(userId,users);
            if (user == 1)
            {
                return new()
                {
                    Result = user,
                    IsSuccess = true,
                    Message = string.Empty
                };
            }
            else
            {
                return new()
                {
                    Result = "No user found with id = " + userId,
                    Message = "No user found with id = " + userId,
                    IsSuccess = false
                };
            }
        }
    }
}
