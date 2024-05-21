using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagementService.Data;
using UserManagementService.Models;
using UserManagementService.Models.DTO;
using UserManagementService.Services.Contract;

namespace UserManagementService.Services.Implementation
{
    public class AuthServices : IAuthServices
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenGenerator _tokenGenerator;

        public AuthServices(AppDbContext appDbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ITokenGenerator tokenGenerator)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<bool> AssignRole(string email, string role)
        {
            var user = await _appDbContext.ApplicationUsers.FirstOrDefaultAsync(m => m.Email.ToLower() == email.ToLower());
            if (user == null)
            {
                return false;
            }
            var roleExist = await _roleManager.RoleExistsAsync(role);
            if (!roleExist)
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
            await _userManager.AddToRoleAsync(user, role);
            return true;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequest)
        {
            var user = await _appDbContext.ApplicationUsers.FirstOrDefaultAsync(m => m.Email.ToLower() == loginRequest.Email.ToLower());
            if (user == null)
            {
                return new()
                {
                    User = null,
                    Token = string.Empty,
                    Message = "Enter the corrct email address."

                };
            }
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (!isValid)
            {
                return new()
                {
                    User = null,
                    Token = string.Empty,
                    Message = "Enter the correct password."
                };
            }
            var roles = await _userManager.GetRolesAsync(user);
            //generates token
            var token = _tokenGenerator.GenerateToken(user, roles);
            UserDto userResponse = new UserDto()
            {
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserId = user.Id,
            };
            return new()
            {
                Token = token,
                User = userResponse,
                Message = "login success"
            };
        }

        public async Task<string> Register([FromBody] RegisterationRequestDto registerationRequest)
        {
            try
            {
                ApplicationUser applicationUser = new ApplicationUser()
                {
                    Email = registerationRequest.Email.ToLower(),
                    NormalizedEmail = registerationRequest.Email.ToUpper(),
                    UserName = registerationRequest.Email,
                    Name = registerationRequest.Name,
                    PhoneNumber = registerationRequest.PhoneNumber,
                    Address = registerationRequest.Address
                };
                var result = await _userManager.CreateAsync(applicationUser, registerationRequest.Password);
                if (result.Succeeded)
                {
                    var userToReturn = _appDbContext.ApplicationUsers.First(m => m.Email == registerationRequest.Email);
                    if (userToReturn != null)
                    {
                        UserDto userDto = new()
                        {
                            Email = userToReturn.Email,
                            UserId = userToReturn.Id,
                            Name = userToReturn.Name,
                            PhoneNumber = userToReturn.PhoneNumber,
                        };
                        return string.Empty;
                    }
                }
                return result.Errors.First().Description;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public async Task<UserDto> GetUserByIdAsync(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var user = await _appDbContext.ApplicationUsers.AsNoTracking().FirstOrDefaultAsync(m => m.Id == userId);
                if (user != null)
                {
                    UserDto userDto = new UserDto()
                    {
                        UserId = userId,
                        Email = user.Email,
                        Name = user.Name,
                        PhoneNumber = user.PhoneNumber,
                    };
                    return userDto;
                }
            }
            return new();
        }
    }
}
