using Ticket_Management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using static Ticket_Management.Models.ApplicationDbContext;
using Ticket_Management.ViewModels;

//[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
    }

    // Admin Dashboard
    public async Task<IActionResult> Index()
    {
        // Get the list of users
        var users = await _userManager.Users.ToListAsync();

        // Create a list of tasks to fetch roles for each user asynchronously
        var userListViewModels = new List<UserListViewModel>();

        foreach (var user in users)
        {
            // For each user, fetch their roles asynchronously
            var roles = await _userManager.GetRolesAsync(user);

            // Add the user along with their roles to the list
            userListViewModels.Add(new UserListViewModel
            {
                Id = user.Id,
                Username = user.UserName,
                Roles = roles.ToList()
            });
        }

        return View(userListViewModels);
    }




    // Create User
    public IActionResult CreateUser()
    {
        // Fetch all roles and pass to the view
        ViewBag.Roles = _roleManager.Roles.ToList();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Create user logic
            var user = new ApplicationUser { UserName = model.Username };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Add roles if available
                if (model.SelectedRoles != null && model.SelectedRoles.Any())
                {
                    await _userManager.AddToRolesAsync(user, model.SelectedRoles);
                }

                return Json(new { success = true });
            }
            else
            {
                // Add model errors
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
        }

        // Handle errors
        var errors = ModelState
            .Where(m => m.Value.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );

        return Json(new { success = false, errors });
    }

    public async Task<IActionResult> EditUser(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        // Get the roles that the user is assigned to
        var roles = await _userManager.GetRolesAsync(user);

        // Get all available roles
        var allRoles = _roleManager.Roles.Select(r => new RoleViewModel { Name = r.Name }).ToList();
        ViewBag.Roles = allRoles;

        var model = new EditUserViewModel
        {
            UserId = user.Id,
            Username = user.UserName,
            SelectedRoles = roles.ToList(), // pre-select the user's roles
            AvailableRoles = allRoles
        };

        return View(model);
    }

    // Edit POST: Admin/EditUser/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(EditUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return Json(new { success = false, errors = new[] { "User not found." } });
            }

            // Update user details
            user.UserName = model.Username;

            // Attempt to update the user
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                // Remove user from existing roles
                var currentRoles = await _userManager.GetRolesAsync(user);
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

                if (removeResult.Succeeded)
                {
                    // Add user to selected roles
                    var addResult = await _userManager.AddToRolesAsync(user, model.SelectedRoles);

                    if (addResult.Succeeded)
                    {
                        // For AJAX: Return success response
                        return Json(new { success = true });
                    }
                    else
                    {
                        // Handle role adding errors
                        var roleErrors = addResult.Errors.Select(e => e.Description).ToArray();
                        return Json(new { success = false, errors = roleErrors });
                    }
                }
                else
                {
                    // Handle role removal errors
                    var removeErrors = removeResult.Errors.Select(e => e.Description).ToArray();
                    return Json(new { success = false, errors = removeErrors });
                }
            }
            else
            {
                // Handle errors from updating the user
                var updateErrors = result.Errors.Select(e => e.Description).ToArray();
                return Json(new { success = false, errors = updateErrors });
            }
        }

        // If model state is not valid, return the validation errors as JSON
        var modelStateErrors = ModelState
            .Where(m => m.Value.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );

        return Json(new { success = false, errors = modelStateErrors });
    }







    // Manage Roles
    [HttpGet]
    public IActionResult ManageRoles()
    {
        ViewBag.Roles = _roleManager.Roles;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            await _roleManager.CreateAsync(new IdentityRole(roleName));
            TempData["Success"] = "Role created successfully!";
        }
        else
        {
            TempData["Error"] = "Role already exists.";
        }
        return RedirectToAction("ManageRoles");
    }

    // Reset Password
    [HttpGet]
    public async Task<IActionResult> ResetPassword(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return NotFound();
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        var model = new ResetPasswordViewModel
        {
            UserId = user.Id,
            Username = user.UserName  // Pre-fill the Username from the database
        };

        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (model.NewPassword != model.ConfirmPassword)
        {
            return Json(new { success = false, message = "The new password and confirmation password do not match." });
        }

        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user != null)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

            if (result.Succeeded)
            {
                return Json(new { success = true, message = "Password reset successfully!" });
            }

            return Json(new { success = false, message = string.Join(", ", result.Errors.Select(e => e.Description)) });
        }

        return Json(new { success = false, message = "User not found." });
    }




}
