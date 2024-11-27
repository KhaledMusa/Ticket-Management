using System.ComponentModel.DataAnnotations;

namespace Ticket_Management.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Employee>? Emps { get; set; }
        public List<Manager>? Managers { get; set; }

        public List<Ticket_Request>? Requests { get; set; }
        public List<Ticket_Replay>? Replays { get; set; }
    }
}
