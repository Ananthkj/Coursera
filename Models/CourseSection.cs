using System.ComponentModel.DataAnnotations;

namespace Coursera.Models
{
    public class CourseSection
    {
        [Key]
        public int Id { get; set; }

        public int CourseId { get; set; }

        public string? CourseSectionName { get; set; }

        public Course? course { get; set; }

        public ICollection<CourseLesson> courseLessons { get; set; }
    }
}
