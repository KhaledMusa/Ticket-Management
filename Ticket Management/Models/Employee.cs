﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticket_Management.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        //public string Salary { get; set; }
        public string? JobDescrption { get; set; }
        [NotMapped]
        public IFormFile? ImageURL { get; set; }
        public string? ImagePath { get; set; }

        //[ForeignKey("Department")]
        public int DepartmentID { get; set; }

        public Department? Department { get; set; }
        [ForeignKey("Manager")]
        public int ManagerId { get; set; }

        public Manager? Manager { get; set; }
        public List<Ticket_Request>? Requests { get; set; }
        public List<Ticket_Replay>? Replays { get; set; }
    }
}
