using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMS.Models
{
    public class RegisterUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }  // Auto Increment

        [Required, StringLength(100)]
        public required string Name { get; set; }

        [Required, EmailAddress, StringLength(150)]
        public required string Email { get; set; }

        [Required, StringLength(255)]
        public required string Password { get; set; }

        [StringLength(15)]
        public string? PhoneNo { get; set; }

        [Required]
        public required string Gender { get; set; }

        [Required]
        public required string Role { get; set; }

        public string Status { get; set; } = "Active";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
