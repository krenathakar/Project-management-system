using System.ComponentModel.DataAnnotations;

namespace PMS.Models
{
    public class User
    {

        [Key] // Primary Key
        public int UserId { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public required string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public required string PasswordHash { get; set; } // store hashed password

        [Required]
        [MaxLength(50)]
        public required string Role { get; set; } // User/Admin
        
    }
}