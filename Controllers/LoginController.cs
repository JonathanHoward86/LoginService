using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LoginService.Models;
using NLog;

namespace LoginService.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public LoginController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            Logger.Info("Entered Login POST method");
             if (model.Email != null && model.Password != null)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("LoginSuccess");
                    }
                }
                ModelState.AddModelError("", "Invalid login attempt");
            }
            else
            {
                ModelState.AddModelError("", "Email and Password must not be null");
            }
            Logger.Info("Exiting Login POST method");
            return View("~/Views/Login/Login.cshtml", model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            Logger.Info("Entered Login GET method");
            var model = new LoginModel();
            return View("~/Views/Login/Login.cshtml", model);
        }

        public IActionResult LoginSuccess()
        {
            Logger.Info("Login successful");
            return View();
        }
    }
}