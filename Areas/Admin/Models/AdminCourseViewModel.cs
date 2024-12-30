using Coursera.Models;

namespace Coursera.Areas.Admin.Models
{
    public class AdminCourseViewModel
    {
        public int courseId {  get; set; }
        public string? courseName { get; set; }

        public string? courseImage {  get; set; }    
        public ApprovalStatus ApprovalStatus { get; set; }
        public bool IsPublished { get; set; }


    }
}
