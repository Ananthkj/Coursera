namespace Coursera.Models
{
    public class MyCoursesViewModel
    {
        public Coursera.Areas.Instructor.Models.MyProfileModel Profile { get; set; }
        public List<Coursera.Models.Course> Courses { get; set; }
    }
}
