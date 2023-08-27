using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LoginService.Models;
using System.Net.Mail;
using NLog;

namespace LoginService.Controllers
{
    public class PasswordController : Controller
    {
        // Declare dependencies to be injected
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        // Constructor for dependency injection
        public PasswordController(UserManager<IdentityUser> userManager, IConfiguration configuration, IEmailService emailService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailService = emailService;
        }

        // POST action for resetting password
        [HttpPost]
        [ValidateAntiForgeryToken] // CSRF protection
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            Logger.Info("Entered ResetPassword POST method");

            // Validate the model
            if (ModelState.IsValid)
            {
                // Check for null email
                if (model.Email == null)
                {
                    ModelState.AddModelError("", "Email must not be null");
                    return View(model);
                }

                // Find the user by email
                var user = await _userManager.FindByEmailAsync(model.Email);

                // If user exists, proceed with password reset
                if (user != null)
                {
                    // Generate a password reset token
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    // Create the reset URL
                    var resetUrl = Url.Action("ResetPasswordConfirm", "Password", new { token, email = model.Email }, Request.Scheme);

                    // Create email body and send email
                    var emailBody = $"Please reset your password by clicking <a href='{resetUrl}'>here</a>.";
                    await _emailService.SendEmail(model.Email, "Reset Password", emailBody);

                    // Redirect to confirmation view
                    return RedirectToAction("ResetPasswordEmailSent");
                }

                // If user not found, add error
                ModelState.AddModelError("", "Email not found");
            }

            Logger.Info("Exiting ResetPassword POST method");
            return View("~/Views/Password/ResetPassword.cshtml", model);
        }

        // GET action to show the reset password form
        [HttpGet]
        public IActionResult ResetPassword()
        {
            Logger.Info("Entered ResetPassword GET method");
            var model = new ResetPasswordModel();
            return View("~/Views/Password/ResetPassword.cshtml", model);
        }

        // View to confirm that the reset password email has been sent
        public IActionResult ResetPasswordEmailSent()
        {
            return View();
        }

        // POST action to confirm the password reset
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPasswordConfirm(ResetPasswordConfirmModel model)
        {
            Logger.Info("Entered ResetPasswordConfirm POST method");

            // Validate the model
            if (ModelState.IsValid)
            {
                Logger.Info($"Valid model state. Email: {model.Email}, Token: {model.Token}");

                // Check for null email
                if (model.Email == null)
                {
                    Logger.Warn("Email is null");
                    ModelState.AddModelError("", "Email must not be null");
                    return View(model);
                }

                // Find the user by email
                var user = await _userManager.FindByEmailAsync(model.Email);

                // If user exists, proceed with password reset
                if (user != null)
                {
                    Logger.Info($"User found: {user.UserName}");

                    // Check for null token and password
                    if (model.Token != null && model.NewPassword != null)
                    {
                        Logger.Info("Token and NewPassword are not null, proceeding to reset password.");

                        // Reset the password
                        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

                        // If successful, redirect to success view
                        if (result.Succeeded)
                        {
                            Logger.Info("Password reset was successful.");
                            return RedirectToAction("ResetPasswordSuccess");
                        }
                        else
                        {
                            // Log and add errors if reset fails
                            Logger.Warn("Password reset failed.");
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                                Logger.Error($"Error during password reset: {error.Description}");
                            }
                        }
                    }
                    else
                    {
                        // Add error for null token or password
                        Logger.Warn("Token and/or New Password is null");
                        ModelState.AddModelError("", "Token and New Password must not be null");
                    }
                }
                else
                {
                    // Add error for user not found
                    Logger.Warn($"User not found for email: {model.Email}");
                }
            }
            else
            {
                // Log invalid model state
                Logger.Warn("Invalid model state.");
            }

            Logger.Info("Exiting ResetPasswordConfirm POST method");
            return View("~/Views/Password/ResetPasswordConfirm.cshtml", model);
        }

        // GET action to show the password reset confirmation form
        [HttpGet]
        public IActionResult ResetPasswordConfirm(string token, string email)
        {
            Logger.Info("Entered ResetPasswordConfirm GET method");
            Logger.Info($"Received token: {token}, email: {email}");
            var model = new ResetPasswordConfirmModel { Token = token, Email = email };
            return View("~/Views/Password/ResetPasswordConfirm.cshtml", model);
        }

        // View to confirm that the password has been successfully reset
        public IActionResult ResetPasswordSuccess()
        {
            return View();
        }

        // POST action to retrieve forgotten username
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotUsername(ForgotUsernameModel model)
        {
            Logger.Info("Entered ForgotUsername POST method");

            // Validate the model
            if (ModelState.IsValid)
            {
                // Check for null email
                if (model.Email == null)
                {
                    ModelState.AddModelError("", "Email must not be null");
                    return View(model);
                }

                // Find the user by email
                var user = await _userManager.FindByEmailAsync(model.Email);

                // If user exists, send username via email
                if (user != null)
                {
                    var username = user.UserName;
                    var emailBody = $"Your username is: {username}";
                    await _emailService.SendEmail(model.Email, "Retrieve Username", emailBody);

                    // Redirect to confirmation view
                    return RedirectToAction("ForgotUsernameEmailSent");
                }

                // If user not found, add error
                ModelState.AddModelError("", "Email not found");
            }

            Logger.Info("Exiting ForgotUsername POST method");
            return View("~/Views/Password/ForgotUsername.cshtml", model);
        }

        // GET action to show the forgotten username form
        [HttpGet]
        public IActionResult ForgotUsername()
        {
            Logger.Info("Entered ForgotUsername GET method");
            var model = new ForgotUsernameModel();
            return View("~/Views/Password/ForgotUsername.cshtml", model);
        }

        // View to confirm that the forgotten username email has been sent
        public IActionResult ForgotUsernameEmailSent()
        {
            Logger.Info("ForgotUsernameEmailSent successful");
            return View();
        }

        // Method to send email
        public async Task SendEmail(string email, string subject, string body)
        {
            // Retrieve SMTP settings from configuration
            var fromEmail = _configuration["SmtpSettings:SmtpFromEmail"];
            var fromName = _configuration["SmtpSettings:SmtpFromName"];

            // Check for null email
            if (fromEmail != null)
            {
                // Configure and send the email
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
                // Log null email
                Logger.Warn("SmtpFromEmail is null");
            }
        }
    }
}