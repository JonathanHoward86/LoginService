using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LoginService.Models;
using NLog;

namespace LoginService.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public RegistrationController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration, ILogger<RegistrationController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                Logger.Info("Entered Register POST method");
                if (model.Email != null && model.Password != null)
                {
                    var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        Logger.Info("User registration succeeded");
                        await _signInManager.SignInAsync(user, false);
                        return RedirectToAction("RegisterSuccess");
                    }
                    foreach (var error in result.Errors)
                    {
                        Logger.Info("User registration failed");
                        ModelState.AddModelError("", error.Description);
                    }
                }
                else
                {
                    Logger.Info("Model state is invalid");
                    ModelState.AddModelError("", "Email and Password must not be null");
                }
            }
            Logger.Info("Exiting Login POST method");
            return View("~/Views/Registration/Register.cshtml", model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            Logger.Info("Entered Register GET method");
            var model = new RegisterModel();
            return View("~/Views/Registration/Register.cshtml", model);
        }

        public IActionResult RegisterSuccess()
        {
            Logger.Info("Register successful");
            return View();
        }
    }
}