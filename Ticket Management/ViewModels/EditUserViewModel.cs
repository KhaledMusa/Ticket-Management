using System.ComponentModel.DataAnnotations;

namespace Ticket_Management.ViewModels
{
    public class EditUserViewModel
    {
        // Identifies the user being edited
        public string UserId { get; set; }

        // Pre-filled with existing user's username
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        // A list of roles the user is assigned to
        [Required(ErrorMessage = "Please assign at least one role.")]
        [Display(Name = "Assign Roles")]
        public List<string> SelectedRoles { get; set; } = new List<string>();

        // List of available roles for the admin to choose from
        public List<RoleViewModel> AvailableRoles { get; set; } = new List<RoleViewModel>();
    }
}
