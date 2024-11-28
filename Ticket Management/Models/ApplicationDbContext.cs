using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ticket_Management.Models
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options ):base(options) 
        { 

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Ticket_Request> Ticket_Requests { get; set; }
        public DbSet<Ticket_Replay> Ticket_Replays { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Employee -> Manager Relationship
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Manager)
                .WithMany(m => m.Employees)
                .HasForeignKey(e => e.ManagerId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent Manager deletion if referenced

            // Manager -> Department Relationship
            modelBuilder.Entity<Manager>()
                .HasOne(m => m.Department)
                .WithMany(d => d.Managers)
                .HasForeignKey(m => m.DepartmentID)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Ticket_Request>()
                .HasOne(m => m.Employee)
                .WithMany(d => d.Requests)
                .HasForeignKey(m => m.EmployeeID)
                .OnDelete(DeleteBehavior.Restrict); 
            
            modelBuilder.Entity<Ticket_Replay>()
                .HasOne(m => m.Request)
                .WithMany(d => d.Replays)
                .HasForeignKey(m => m.TicketRequestID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket_Replay>()
                .HasOne(m => m.Employee)
                .WithMany(d => d.Replays)
                .HasForeignKey(m => m.EmployeeID)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }

}
