using System.ComponentModel.DataAnnotations;

namespace Coursera.Models
{
    public class ContactViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [Display(Name = "Name")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Subject is Required.")]
        public string? Subject { get; set; }

        [Required(ErrorMessage = "Message is Required.")]
        public string? Message { get; set; }

    }
}
