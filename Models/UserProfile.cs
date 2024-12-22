using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coursera.Models
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }

        public string? Photo { get; set; }

        public string? Subject { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public string? Website { get; set; }

        public string? Twitter { get; set; }

        public string? Facebook { get; set; }

        public string? LinkedIn { get; set; }

        public string? Instagram { get; set; }

        public User? user { get; set; }
    }
}
