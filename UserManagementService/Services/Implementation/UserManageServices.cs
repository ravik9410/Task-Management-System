using Mango.Services.CouponAPI.Data;
using Microsoft.EntityFrameworkCore;
using UserManagementService.Models;
using UserManagementService.Models.DTO;
using UserManagementService.Services.Contract;

namespace UserManagementService.Services.Implementation
{
    public class UserManageServices : IUserManage
    {
        private readonly AppDbContext _appDbContext;

        public UserManageServices(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<int> DeleteUserAsync(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var user = _appDbContext.ApplicationUsers.FirstOrDefault(m => m.Id == userId);
                if (user != null)
                {
                    _appDbContext.ApplicationUsers.Remove(user);
                    return await _appDbContext.SaveChangesAsync();
                }
            }
            return 0;
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

        public async Task<List<UserDto>> GetUsersAsync()
        {
            var users = await _appDbContext.ApplicationUsers.AsNoTracking().ToListAsync();
            List<UserDto> result = new List<UserDto>();
            foreach (var item in users)
            {
                UserDto userDto = new UserDto()
                {
                    Name = item.Name,
                    Email = item.Email,
                    PhoneNumber = item.PhoneNumber,
                    UserId = item.Id
                };
                result.Add(userDto);
            }
            return result;
        }

        public async Task<int> UpdateUserAsync(string userId, UserDto users)
        {
            if (userId != users.UserId)
            {
                return 0;
            }
            var userDetails = await _appDbContext.ApplicationUsers.FirstOrDefaultAsync(m => m.Id == userId);
            if (userDetails != null)
            {
                userDetails.Name = users.Name;
                userDetails.PhoneNumber = users.PhoneNumber;
            }
            await _appDbContext.SaveChangesAsync();
            return 1;
        }
    }
}
