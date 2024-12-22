using System.ComponentModel.DataAnnotations;

namespace Coursera.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string? RoleName {  get; set; }

        //For the User entity(Multiple instances) single role can be given
        public ICollection<User>? Users { get; set; }
    }
}
