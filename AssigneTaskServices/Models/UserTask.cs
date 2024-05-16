using System.ComponentModel.DataAnnotations;
#nullable disable
namespace AssigneTaskServices.Models
{
    public class UserTask
    {
        [Key]       
        public int TaskId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        [Required]
        public string UserId { get; set; }
        public string TaskStatus { get; set; }
        public string TaskAssignedBy { get; set; }
    }
}
