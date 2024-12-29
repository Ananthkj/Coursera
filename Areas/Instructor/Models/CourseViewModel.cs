using Coursera.Models;

namespace Coursera.Areas.Instructor.Models
{
    public class CourseViewModel
    {
        public string CourseName {  get; set; }

        public string CourseImage {  get; set; }
        public ApprovalStatus ApprovalStatus {  get; set; }
        public bool IsPublished {  get; set; }
    }
}
