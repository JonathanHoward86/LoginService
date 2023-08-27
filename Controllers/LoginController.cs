using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LoginService.Models;
using NLog;

namespace LoginService.Controllers
{
    // LoginController class inherits from Controller class
    public class LoginController : Controller
    {
        // Declare UserManager and SignInManager for Identity framework
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        // Initialize NLog logger
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        // Constructor to inject UserManager and SignInManager
        public LoginController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // POST method for login
        [HttpPost]
        [ValidateAntiForgeryToken] // Anti-forgery token to prevent CSRF attacks
        public async Task<IActionResult> Login(LoginModel model)
        {
            // Log entry point of Login POST method
            Logger.Info("Entered Login POST method");

            // Check if Email and Password are not null
            if (model.Email != null && model.Password != null)
            {
                // Find user by email
                var user = await _userManager.FindByEmailAsync(model.Email);

                // Check if user exists
                if (user != null)
                {
                    // Attempt to sign in the user
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

                    // If sign-in is successful, redirect to success page
                    if (result.Succeeded)
                    {
                        return RedirectToAction("LoginSuccess");
                    }
                }

                // Add error to ModelState if login fails
                ModelState.AddModelError("", "Invalid login attempt");
            }
            else
            {
                // Add error to ModelState if Email or Password is null
                ModelState.AddModelError("", "Email and Password must not be null");
            }

            // Log exit point of Login POST method
            Logger.Info("Exiting Login POST method");

            // Return to login view with model
            return View("~/Views/Login/Login.cshtml", model);
        }

        // GET method for login
        [HttpGet]
        public IActionResult Login()
        {
            // Log entry point of Login GET method
            Logger.Info("Entered Login GET method");

            // Initialize new LoginModel
            var model = new LoginModel();

            // Return login view with model
            return View("~/Views/Login/Login.cshtml", model);
        }

        // Action for successful login
        public IActionResult LoginSuccess()
        {
            // Log successful login
            Logger.Info("Login successful");

            // Return success view
            return View();
        }
    }
}
