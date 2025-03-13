using System.ComponentModel.DataAnnotations;

namespace CollegeApp.Models
{
    public class RolePrivilegeDTO
    {
        public int Id { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        [MaxLength(100)]
        public string RolePrivilegeName { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }
        [Required]

        public bool IsActive { get; set; } 
    }
}

