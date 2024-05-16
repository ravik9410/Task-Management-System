using System.ComponentModel.DataAnnotations;

namespace AssigneTaskServices.Models
{
    public class AssignedUserTask
    {
        [Key]
        public int AssignedId { get; set; }
        [Required]
        public int TaskId { get; set; }
        [Required]
        public string UserId { get; set; } = string.Empty;
        public string TaskStatus { get; set; }=string.Empty;
        public string TaskAssignedBy { get; set; }=string.Empty;
    }
}
