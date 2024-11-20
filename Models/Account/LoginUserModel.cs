using System.ComponentModel.DataAnnotations;

namespace Coursera.Models.Account
{
    public class LoginUserModel
    {
        [Required(ErrorMessage ="Email is Mandatory")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage ="Password is Mandatory")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
