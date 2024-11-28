using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticket_Management.Models
{
    public class Ticket_Request
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string Attatchment { get; set; }

        [ForeignKey("Department")]
        public int DepartmentID { get; set; }
        public Department? Department { get; set; }


        [ForeignKey("Employee")]
        public int EmployeeID { get; set; }
        public Employee? Employee { get; set; }

        public List<Ticket_Replay>? Replays { get; set; }
    }
}
