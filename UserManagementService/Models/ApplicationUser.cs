using Microsoft.AspNetCore.Identity;
#nullable disable
namespace UserManagementService.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
