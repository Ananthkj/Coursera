namespace Coursera.Areas.Instructor.Models
{
    public class AddLessonViewModel
    {
        public int CourseId { get; set; }
        public string? CourseLessonName { get; set; }

        public string? ContentUrl { get; set; }

        public string? ContentType { get; set; }
    }
}
