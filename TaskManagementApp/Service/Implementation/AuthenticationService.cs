using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaskManagementApp.Models.DTO;
using TaskManagementApp.Service.Contract;
using TaskManagementApp.Utility;
using static TaskManagementApp.Utility.StaticData;
namespace TaskManagementApp.Service.Implementation
{
    public class AuthenticationService : IAuthentication
    {
        private readonly IBaseServices _baseService;
        private readonly ITokenHandler _tokenHandler;
        private readonly IHttpContextAccessor _contextAccessor;
        public AuthenticationService(IBaseServices baseService, ITokenHandler tokenHandler, IHttpContextAccessor contextAccessor)
        {
            _baseService = baseService;
            _tokenHandler = tokenHandler;
            _contextAccessor = contextAccessor;
        }

        public async Task<UserDto> Login(LoginRequestDto login)
        {

            var result = await _baseService.SendAsync(new()
            {
                ApiType = ApiType.POST,
                Url = AuthUrl + "/api/auth/login",
                Data = login
            });
            if (result!.IsSuccess)
            {
                var responseContent = JsonConvert.DeserializeObject<LoginResponseDto>(result.Result?.ToString()!);
                //var user = JsonConvert.DeserializeObject<UserDto>(responseContent?.User?.ToString()!);
                SignIn(responseContent?.User!, responseContent?.Token!);
                return responseContent?.User!;
                //login success
            }
            else
            {
                return new();
            }
            //return new();
        }

        public async Task Logout()
        {
            await _contextAccessor.HttpContext?.SignOutAsync(null, null)!;
        }

        public async Task<string> Registration(RegisterationRequestDto register)
        {
            var result = await _baseService.SendAsync(new()
            {
                ApiType = ApiType.POST,
                Url = AuthUrl + "/api/auth/register",
                Data = register
            });
            if (result!.IsSuccess)
        {
                var results = await _baseService.SendAsync(new()
                {
                    ApiType = ApiType.POST,
                    Url = AuthUrl + "/api/auth/AssignRole",
                    Data = register
                });
                if (results!.IsSuccess)
                {
                    //var response= results.Result?.ToString()!;

                    return result.Result?.ToString()?.Trim()!;

        }
                else
                {
                    return result.Message!;
                }
                return result.Message!;
                //return JsonConvert.DeserializeObject<string>(result.Result?.ToString()!)!;

            }
            else
            {
                return result.Message!;
            }
            //  return result.Message!;
        }
        private void SignIn(UserDto user, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _tokenHandler.SetToken(token);
                var tokenReader = new JwtSecurityTokenHandler();
                var jwt = tokenReader.ReadJwtToken(token);
                if (jwt != null)
        {
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.Email, jwt.Claims.FirstOrDefault(m => m.Type == JwtRegisteredClaimNames.Email)!.Value));
                    identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(m => m.Type == "role")!.Value));
                    identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(m => m.Type == JwtRegisteredClaimNames.Name)!.Value));
                    identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(m => m.Type == JwtRegisteredClaimNames.Sub)!.Value));
                    ClaimsPrincipal principal = new ClaimsPrincipal(new[] { identity });
                    _ = _contextAccessor.HttpContext?.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal: principal, properties: new AuthenticationProperties() { IsPersistent = false, ExpiresUtc = DateTime.UtcNow.AddDays(1) });
                }
            }
        }
    }
}
