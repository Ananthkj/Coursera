using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coursera.Models
{
    public class UniversityFormModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Name is required")]
        [Display(Name ="First Name")]
        [StringLength(50,ErrorMessage ="Should not exceed 50 charcters")]
        public string? Fname { get; set; }


        [Required(ErrorMessage ="Lname is Mandatory")]
        [Display(Name ="Last Name")]
        [StringLength(50,ErrorMessage = "Should not exceed 50 charcters")]
        public string? Lname { get; set; }

        [Required(ErrorMessage ="Email is Mandatory")]
        [Display(Name ="EmailId")]
        [EmailAddress(ErrorMessage ="Invalid Email Format")]
        public string? Email { get; set; }

   
        public string? Phone { get; set; }

    
        public string? Gender { get; set; }

       
        public DateTime DateOfBirth { get; set; }


       
        public string? City { get; set; }

       
        public string? State { get; set; }

       
        public string? Country { get; set; }

        [NotMapped]
        [Required(ErrorMessage ="File is Mandatory")]
        [Validator(ErrorMessage ="File Required")]
        public IFormFile? File { get; set; }

        [NotMapped]
        [Range(1, 5 * 1024 * 1024, ErrorMessage = "File size cannot exceed 5MB.")]
        public long FileSize => File?.Length ?? 0;
    }
}
