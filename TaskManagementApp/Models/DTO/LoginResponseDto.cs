namespace TaskManagementApp.Models.DTO
{
    public class LoginResponseDto
    {
        public UserDto? User { get; set; }
        public string Token { get; set; }=string.Empty;
        public string? Message { get; set; }

    }
}
