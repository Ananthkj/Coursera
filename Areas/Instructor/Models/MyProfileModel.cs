using Coursera.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coursera.Areas.Instructor.Models
{
    public class MyProfileModel
    {
        public string? UserName {  get; set; }

        public string? RoleName {  get; set; }

        public string? Email {  get; set; }
        public string? Photo { get; set; }

        public string? Subject { get; set; }
      
        public int UserId { get; set; }

        public string? Website { get; set; }

        public string? Twitter { get; set; }

        public string? Facebook { get; set; }

        public string? LinkedIn { get; set; }

        public string? Instagram { get; set; }

        public List<Coursera.Models.Course> Courses { get; set; }

        public List<Coursera.Areas.Instructor.Models.CourseViewModel> AllCourses { get; set; }

    }
}
