using IdeaExchange.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Events;
using System.Security.Claims;

namespace IdeaExchange.Controllers
{
    public class ApplicationUserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<ApplicationUser> _logger;

        public ApplicationUserController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
            ILogger<ApplicationUser> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("register/post")]
        public async Task<IActionResult> Post(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    Log.Information("User registered successfully: {UserName}", user.UserName);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    Log.Error("User registration failed for {UserName}: {Errors}", user.UserName, result.Errors.Select(e => e.Description));
                    return View("Error", result.Errors.Select(e => e.Description));
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);

                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    Log.Information("Login successful for user: {Username}", model.Username);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    Log.Warning("Login failed for user: {Username}", model.Username);
                    return View("LoginFailed", model);
                }
            }
            else
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();

                Log.Information("Successful logout");

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Log.Warning("Logout failed");
                return View("Error", ex);
            }
        }

        [Route("applicationuser/userprofile/{id}")]
        public async Task<IActionResult> UserProfile(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                Log.Warning("Null userId");
                return BadRequest();
            }

            var user = await _userManager.FindByIdAsync(Id);

            if (user == null)
            {
                return NotFound();
            }

            var userProfileViewModel = new UserProfileViewModel
            {
                UserName = user.UserName
            };

            return View(userProfileViewModel);
        }
    }
}
