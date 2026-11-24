using System.ComponentModel.DataAnnotations;

namespace SmartCampusServicePortal.Models
{
    public class ServiceRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string StudentName { get; set; } = string.Empty;

        public string? StudentRollNo { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";

        public string? AdminRemarks { get; set; }

        public DateTime SubmissionDate { get; set; } = DateTime.Now;
    }
}
