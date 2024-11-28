using Ticket_Management.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static Ticket_Management.Models.ApplicationDbContext;
using Ticket_Management.ViewModels;

namespace Ticket_Management.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        // Constructor to inject SignInManager and UserManager
        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the user by username (you may use email if needed)
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user != null)
                {
                    // Sign in the user
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home"); // Redirect to Home page or a different page after successful login
                    }
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            return View(model);
        }

        // GET: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account"); // Redirect to the login page after logout
        }

        // GET: /Account/AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
