using System.ComponentModel.DataAnnotations;

namespace Coursera.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required, EmailAddress]
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public DateTime CreatedDate { get; set; }

        public bool IsActive {  get; set; }
        [Required]
        public int RoleId { get; set; }

        //Single entity means single role can be given
        public Role? Role { get; set; }

    }
}
