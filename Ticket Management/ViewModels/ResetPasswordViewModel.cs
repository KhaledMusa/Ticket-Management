using System.ComponentModel.DataAnnotations;

namespace Ticket_Management.ViewModels
{
    public class ResetPasswordViewModel
    {
        public string UserId { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please confirm your password.")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
