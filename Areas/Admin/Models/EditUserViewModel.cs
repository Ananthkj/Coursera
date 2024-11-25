using System.ComponentModel.DataAnnotations;

namespace Coursera.Areas.Admin.Models
{
    public class EditUserViewModel
    {

        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
