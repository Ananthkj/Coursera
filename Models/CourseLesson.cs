using System.ComponentModel.DataAnnotations;

namespace Coursera.Models
{
    public class CourseLesson
    {
        [Key]
        public int Id { get; set; }

        public int CourseId { get; set; }

        public string? CourseLessonName {  get; set; }

        public string? ContentUrl {  get; set; }

        public string? ContentType {  get; set; }

        public CourseSection? Section { get; set; }
    }
}
