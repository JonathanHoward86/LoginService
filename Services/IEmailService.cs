namespace LoginService.Models
{
    // Interface for email service
    public interface IEmailService
    {
        // Method signature for sending email
        Task SendEmail(string email, string subject, string body);
    }
}