using System.ComponentModel.DataAnnotations;

namespace Coursera.Areas.Instructor.Models
{
    public class AddCourseViewModel
    {
        [Required]
        public string CourseName {  get; set; }

        [Required]
        public string CourseDescription { get; set; }

        [Required(ErrorMessage ="Please Upload a Image")]
        public IFormFile formFile { get; set; }
    }
}
