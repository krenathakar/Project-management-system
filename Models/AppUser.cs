using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PMS.Models
{
    public enum Role
    {
        User = 0,
        Admin = 1
    }

    public class AppUser: IdentityUser
    {
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string FullName { get; set; } = "";

        [Required, EmailAddress, StringLength(120)]
        public override string Email { get; set; } = "";

        [Required] // store a hash, not plain text
        public string PasswordHash { get; set; } = "";

        [Required]
        public Role Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
