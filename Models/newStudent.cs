using System.ComponentModel.DataAnnotations;

namespace Coursera.Models
{
    public class newStudent
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Contact Name is Mandatory..")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Contact Email is Mandatory..")]
        [EmailAddress(ErrorMessage = "Invalid email Id..")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Course is Mandatory..")]
        public string? Course { get; set; }

        [Required(ErrorMessage ="Select a Course")]
        public string? subCourse {  get; set; }
    }
}
