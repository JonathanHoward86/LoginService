using System.ComponentModel.DataAnnotations;

namespace LoginService.Models
{
    public class ForgotUsernameModel // Model to handle forgotten username request.
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; } // Required email property.
    }
}