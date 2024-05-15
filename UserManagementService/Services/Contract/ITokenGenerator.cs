using UserManagementService.Models;

namespace UserManagementService.Services.Contract
{
    public interface ITokenGenerator
    {
        string GenerateToken(ApplicationUser User, IEnumerable<string> roles);
    }
}
