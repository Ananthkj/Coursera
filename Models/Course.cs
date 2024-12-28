using System.ComponentModel.DataAnnotations;

namespace Coursera.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        public string CourseName {  get; set; }

        public string CourseImage {  get; set; }

        public string CourseDescription { get; set; }

        public int InstructorId {  get; set; }

        public DateTime CreatedDate { get; set; }

        public ApprovalStatus ApprovalStatus {  get; set; }

        public bool IsPublished {  get; set; }


        public User Instructor { get; set; }
        public ICollection<CourseSection> sections { get; set; }
    }
}
