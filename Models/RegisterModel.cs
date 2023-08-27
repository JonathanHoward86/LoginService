using System.ComponentModel.DataAnnotations;

namespace LoginService.Models
{
    public class RegisterModel // Model for user registration.
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; } // Required email property.
        [Required]
        [MinLength(6)] // Minimum length for a password, change as needed.
        public string? Password { get; set; } // Required password property.
        [Compare("Password")] // Ensures this matches the password field.
        public string? ConfirmPassword { get; set; } // Required confirm password property.
    }
}