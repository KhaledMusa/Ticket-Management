using System.ComponentModel.DataAnnotations;

namespace Ticket_Management.ViewModels
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please assign at least one role.")]
        [Display(Name = "Assign Roles")]
        public List<string> SelectedRoles { get; set; } = new List<string>();

        public List<RoleViewModel> AvailableRoles { get; set; } = new List<RoleViewModel>();
    }

    public class RoleViewModel
    {
        public string Name { get; set; }
    }
}
