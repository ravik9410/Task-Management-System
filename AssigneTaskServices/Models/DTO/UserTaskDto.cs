using System.ComponentModel.DataAnnotations;
namespace AssigneTaskServices.Models.DTO
{
    public class UserTaskDto
    {
        public int TaskId { get; set; } = 0;
        public string? Title { get; set; } 
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string? UserId { get; set; }
        public string? TaskStatus { get; set; }
        public string? TaskAssignedBy { get; set; }
    }

}
