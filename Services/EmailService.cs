using System.Net.Mail;
using NLog;

namespace LoginService.Models
{
    public class EmailService : IEmailService
    {
        // Declare dependencies to be injected
        private readonly IConfiguration _configuration;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        // Constructor for dependency injection
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Method to send email
        public async Task SendEmail(string email, string subject, string body)
        {
            // Retrieve email settings from configuration
            var fromEmail = _configuration["SmtpSettings:SmtpFromEmail"];
            var fromName = _configuration["SmtpSettings:SmtpFromName"];

            // Check for null email address
            if (fromEmail != null)
            {
                // Initialize SMTP client and MailMessage
                using (var client = new SmtpClient("localhost", 1025))
                using (var message = new MailMessage())
                {
                    // Set email properties
                    message.From = new MailAddress(fromEmail, fromName);
                    message.To.Add(email);
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;

                    // Send email
                    client.UseDefaultCredentials = false;
                    await client.SendMailAsync(message);
                }
            }
            else
            {
                // Log null email
                Logger.Warn("SmtpFromEmail is null");
            }
        }
    }
}