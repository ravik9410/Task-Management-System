#nullable disable
namespace UserManagementService.Models.DTO
{
    public class RegisterationRequestDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Address { get; set; }
    }
}
