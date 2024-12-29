using Coursera.Models;
using System.ComponentModel.DataAnnotations;

namespace Coursera.Areas.Instructor.Models
{
    public class CourseViewModel
    {
        public int CourseId {  get; set; }
       
        public string CourseName {  get; set; }
  
        public string CourseImage {  get; set; }

      
        public IFormFile CourseImageFile { get; set; }

       
        public ApprovalStatus ApprovalStatus {  get; set; }

     
        public bool IsPublished {  get; set; }
    }
}
