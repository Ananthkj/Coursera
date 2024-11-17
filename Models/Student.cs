using System.ComponentModel.DataAnnotations;

namespace Coursera.Models
{
    public class Student
    {
        [Required(ErrorMessage = "First Name is required..")]
        [StringLength(100)]
        public string? Fname { get; set; }


        [Required(ErrorMessage = "Lname is required..")]
        [StringLength(100)]
        public string? Lname { get; set; }

        [Required(ErrorMessage = "Email is required..")]
       /* [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+[a-zA-Z0-9-.]$")]*/
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Phone number is required..")]
        [RegularExpression(@"^[+-]\d{2}\s?\d{10}$")]
        [Display(Name = "Phone Number")]
        [Phone]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Gender is required..")]
        [Display(Name = "Gender")]
        public string? Gender { get; set; }

        [Required(ErrorMessage = "Data of Birth is required..")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }


        [Required(ErrorMessage = "City is required..")]
        public string? City { get; set; }

        [Required(ErrorMessage = "State is required..")]
        public string? State { get; set; }

        [Required(ErrorMessage = "Country is required..")]
        public string? Country { get; set; }

        [Required(ErrorMessage = "File is required.")]
        [FileTypeValidation(ErrorMessage = "File type should be jpg, jpeg, or png format.")]
        [Display(Name = "File")]
        public IFormFile? File { get; set; }


        [Range(1, 5 * 1024 * 1024, ErrorMessage = "File size cannot exceed 5MB.")]
        public long FileSize => File?.Length ?? 0;
    }
}
    