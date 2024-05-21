using System.ComponentModel.DataAnnotations;

namespace TaskManagementApp.Models.DTO
{
    public class AssignedUserTaskDto
    {        
        public int AssignedId { get; set; }        
        public int TaskId { get; set; }       
        public string UserId { get; set; } = string.Empty;
        public string TaskStatus { get; set; } = string.Empty;
        public string TaskAssignedBy { get; set; } = string.Empty;
    }
}
