using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LoginService.Models;
using NLog;

namespace LoginService.Controllers
{
    public class RegistrationController : Controller
    {
        // Declare dependencies to be injected
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        // Constructor for dependency injection
        public RegistrationController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        // POST action for user registration
        [HttpPost]
        [ValidateAntiForgeryToken] // CSRF protection
        public async Task<IActionResult> Register(RegisterModel model)
        {
            // Validate the model
            if (ModelState.IsValid)
            {
                Logger.Info("Entered Register POST method");

                // Check for null email and password
                if (model.Email != null && model.Password != null)
                {
                    // Create a new IdentityUser object
                    var user = new IdentityUser { UserName = model.Email, Email = model.Email };

                    // Attempt to create the user
                    var result = await _userManager.CreateAsync(user, model.Password);

                    // If successful, sign in the user and redirect to success view
                    if (result.Succeeded)
                    {
                        Logger.Info("User registration succeeded");
                        await _signInManager.SignInAsync(user, false);
                        return RedirectToAction("RegisterSuccess");
                    }

                    // If registration fails, log and add errors
                    foreach (var error in result.Errors)
                    {
                        Logger.Info("User registration failed");
                        ModelState.AddModelError("", error.Description);
                    }
                }
                else
                {
                    // Add error for null email or password
                    Logger.Info("Model state is invalid");
                    ModelState.AddModelError("", "Email and Password must not be null");
                }
            }

            // Log exit from POST method
            Logger.Info("Exiting Login POST method");
            return View("~/Views/Registration/Register.cshtml", model);
        }

        // GET action to show the registration form
        [HttpGet]
        public IActionResult Register()
        {
            Logger.Info("Entered Register GET method");
            var model = new RegisterModel();
            return View("~/Views/Registration/Register.cshtml", model);
        }

        // View to confirm that the registration has been successful
        public IActionResult RegisterSuccess()
        {
            Logger.Info("Register successful");
            return View();
        }
    }
}