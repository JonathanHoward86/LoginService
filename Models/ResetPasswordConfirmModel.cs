using System.ComponentModel.DataAnnotations;

namespace LoginService.Models
{
    public class ResetPasswordConfirmModel
    {
        // Token for password reset
        public string? Token { get; set; }

        // Email of the user
        public string? Email { get; set; }

        // New password to be set
        [Required]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        // Confirmation of the new password
        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}