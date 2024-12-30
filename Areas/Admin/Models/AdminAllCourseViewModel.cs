using Coursera.Models;

namespace Coursera.Areas.Admin.Models
{
    public class AdminAllCourseViewModel
    {
        public int CourseId { get; set; }

        public string CourseName { get; set; }

        public string CourseImage { get; set; }

        public ApprovalStatus ApprovalStatus { get; set; }

        public bool IsPublished { get; set; }
    }
}
