using System.ComponentModel.DataAnnotations;

namespace Coursera.Areas.Instructor.Models
{
    public class AddLessonViewModel
    {
        [Required]
        public int SectionId { get; set; }
        
        [Required]
        public string? CourseLessonName { get; set; }

        [Required]
        public string? ContentUrl { get; set; }

        [Required]
        public string? ContentType { get; set; }
    }
}
