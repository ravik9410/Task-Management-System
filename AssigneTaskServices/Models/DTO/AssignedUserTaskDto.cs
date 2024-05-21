using System.ComponentModel.DataAnnotations;

namespace AssigneTaskServices.Models.DTO
{
    public class AssignedUserTaskDto
    {
        public int? AssignedId { get; set; } = 0;
        public int? TaskId { get; set; } = 0;
        public string? UserId { get; set; } = string.Empty;
        public string? TaskStatus { get; set; } = string.Empty;
        public string? TaskAssignedBy { get; set; } = string.Empty;
    }
}
