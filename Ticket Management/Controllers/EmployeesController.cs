using Microsoft.AspNetCore.Mvc;
using Ticket_Management.Models;
using Ticket_Management.Services;

namespace Ticket_Management.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IService<Employee> _Employee;

        public EmployeesController(IService<Employee> employeeService)
        {
            _Employee = employeeService;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await _Employee.GetAllAsync();
            return View(employees);
        }

        // GET: employees/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var employees = await _Employee.GetByIdAsync(id);
            if (employees == null)
            {
                return NotFound();
            }
            return View(employees);
        }

        // GET: employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employees)
        {
            if (ModelState.IsValid)
            {
                await _Employee.AddAsync(employees);
                return RedirectToAction(nameof(Index));
            }
            return View(employees);
        }

        // GET: employees/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var employees = await _Employee.GetByIdAsync(id);
            if (employees == null)
            {
                return NotFound();
            }
            return View(employees);
        }

        // POST: employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employees)
        {
            if (id != employees.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _Employee.UpdateAsync(employees);
                return RedirectToAction(nameof(Index));
            }
            return View(employees);
        }

        // GET: employees/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var employees = await _Employee.GetByIdAsync(id);
            if (employees == null)
            {
                return NotFound();
            }
            return View(employees);
        }

        // POST: employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _Employee.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
    
