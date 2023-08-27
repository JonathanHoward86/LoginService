using System.ComponentModel.DataAnnotations;

namespace LoginService.Models
{
    public class ResetPasswordConfirmModel
    {
        public string? Token { get; set; }
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}