using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LoginService.Models;
using System.Net.Mail;
using NLog;

namespace LoginService.Controllers
{
    public class PasswordController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public PasswordController(UserManager<IdentityUser> userManager, IConfiguration configuration, IEmailService emailService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            Logger.Info("Entered ResetPassword POST method");
            if (ModelState.IsValid)
            {
                if (model.Email == null) // Check if Email is null
                {
                    ModelState.AddModelError("", "Email must not be null");
                    return View(model);
                }
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var resetUrl = Url.Action("ResetPasswordConfirm", "Account", new { token, email = model.Email }, Request.Scheme);
                    var emailBody = $"Please reset your password by clicking <a href='{resetUrl}'>here</a>.";
                    await _emailService.SendEmail(model.Email, "Reset Password", emailBody);
                    return RedirectToAction("ResetPasswordEmailSent");
                }
                ModelState.AddModelError("", "Email not found");
            }
            Logger.Info("Exiting ResetPassword POST method");
            return View("~/Views/Password/ResetPassword.cshtml", model);
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            Logger.Info("Entered ResetPassword GET method");
            var model = new ResetPasswordModel();
            return View("~/Views/Password/ResetPassword.cshtml", model);
        }

        public IActionResult ResetPasswordEmailSent()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirm(string token, string email)
        {
            Logger.Info("Entered ResetPasswordConfirm GET method");
            var model = new ResetPasswordConfirmModel { Token = token, Email = email };
            return View("~/Views/Password/ResetPasswordConfirm.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordConfirm(ResetPasswordConfirmModel model)
        {
            Logger.Info("Entered ResetPasswordConfirm POST method");
            if (ModelState.IsValid)
            {
                if (model.Email == null) // Check if Email is null
                {
                    ModelState.AddModelError("", "Email must not be null");
                    return View(model);
                }
                
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    if (model.Token != null && model.NewPassword != null)
                    {
                        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("ResetPasswordSuccess");
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Token and New Password must not be null");
                    }
                }
            }
            Logger.Info("Exiting ResetPasswordConfirm POST method");
            return View("~/Views/Password/ResetPasswordConfirm.cshtml", model);
        }

        public IActionResult ResetPasswordSuccess()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotUsername(ForgotUsernameModel model)
        {
            Logger.Info("Entered ForgotUsername POST method");
            if (ModelState.IsValid)
            {
                if (model.Email == null)
                {
                    ModelState.AddModelError("", "Email must not be null");
                    return View(model);
                }
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var username = user.UserName;
                    var emailBody = $"Your username is: {username}";
                    await _emailService.SendEmail(model.Email, "Retrieve Username", emailBody);
                    return RedirectToAction("ForgotUsernameEmailSent");
                }
                ModelState.AddModelError("", "Email not found");
            }
            Logger.Info("Exiting ForgotUsername POST method");
            return View("~/Views/Password/ForgotUsername.cshtml", model);
        }

        [HttpGet]
        public IActionResult ForgotUsername()
        {
            Logger.Info("Entered ForgotUsername GET method");
            var model = new ForgotUsernameModel();
            return View("~/Views/Password/ForgotUsername.cshtml", model);
        }

        public IActionResult ForgotUsernameEmailSent()
        {
            Logger.Info("ForgotUsernameEmailSent successful");
            return View();
        }

        public async Task SendEmail(string email, string subject, string body)
        {
            var fromEmail = _configuration["SmtpSettings:SmtpFromEmail"];
            var fromName = _configuration["SmtpSettings:SmtpFromName"];
            if (fromEmail != null)  // Check for null
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