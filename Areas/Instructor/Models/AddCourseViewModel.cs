using System.ComponentModel.DataAnnotations;

namespace Coursera.Areas.Instructor.Models
{
    public class AddCourseViewModel
    {
        [Required]
        public string CourseName {  get; set; }

        [Required]
        public string CourseDescription { get; set; }

        
        public IFormFile formFile { get; set; }
    }
}
