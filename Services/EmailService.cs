using System.Net.Mail;

namespace LoginService.Models
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmail(string email, string subject, string body)
        {
            var fromEmail = _configuration["SmtpSettings:SmtpFromEmail"];
            var fromName = _configuration["SmtpSettings:SmtpFromName"];
            if (fromEmail != null)
            {
                using (var client = new SmtpClient("localhost", 1025))
                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(fromEmail, fromName);
                    message.To.Add(email);
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;
                    client.UseDefaultCredentials = false;
                    await client.SendMailAsync(message);
                }
            }
            else
            {
                // Handle null email address
            }
        }
    }
}