namespace LoginService.Models
{
    public interface IEmailService
    {
        Task SendEmail(string email, string subject, string body);
    }
}