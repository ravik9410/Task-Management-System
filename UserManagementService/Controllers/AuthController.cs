using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagementService.Models.DTO;
using UserManagementService.Services.Contract;

namespace UserManagementService.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authServices;
        private ResponseDto _responseDto;

        public AuthController(IAuthServices authServices)
        {
            _authServices = authServices;
            _responseDto = new ResponseDto();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto loginUser)
        {
            var result = await _authServices.Login(loginUser);
            if (result.User == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = result.Message;

                return BadRequest(_responseDto);
            }
            _responseDto.Result = result;
            return Ok(_responseDto);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterationRequestDto registerNew)
        {
            var result = await _authServices.Register(registerNew);
            if (!string.IsNullOrEmpty(result))
            {
                _responseDto.Message = result;
                _responseDto.IsSuccess = false;
                return BadRequest(_responseDto);
            }
            return Ok(_responseDto);
        }
        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole(RegisterationRequestDto registerNew)
        {
            var result = await _authServices.AssignRole(registerNew.Email, registerNew.Role.ToUpper());
            if (!result)
            {
                _responseDto.Message = "Error encounter";
                _responseDto.IsSuccess = false;
                return BadRequest(_responseDto);
            }
            return Ok(_responseDto);
        }
    }
}
