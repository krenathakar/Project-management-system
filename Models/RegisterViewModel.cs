using System.ComponentModel.DataAnnotations;

namespace PMS.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [StringLength(150)]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(15)]
        public string? PhoneNo { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public required string Gender { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public required string Role { get; set; }
    }
}
