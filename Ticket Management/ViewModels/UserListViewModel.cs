namespace Ticket_Management.ViewModels
{
    public class UserListViewModel
    {
        public string Id { get; set; }             // User ID
        public string Username { get; set; }       // User's username
        public List<string> Roles { get; set; }    // List of roles assigned to the user
    }
}
