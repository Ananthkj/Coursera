using System.ComponentModel.DataAnnotations;

namespace Coursera.Models.Account
{
    public class SignUpUserModel
    {
        [Required(ErrorMessage ="User Name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage ="Email is required")]
        [EmailAddress]
        public string Email {  get; set; }

        [Required(ErrorMessage ="Password is Mandatory")]
        [MinLength(6,ErrorMessage = "Password must be at least 6 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage ="Confirm Password is Mandatory")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword {  get; set; }
    }
}
