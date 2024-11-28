using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ticket_Management.Models;
using Ticket_Management.Services;

namespace Ticket_Management.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IService<Employee> _Employee;
        private readonly IService<Department> _Department;
        private readonly IService<Manager> _Manager;

        public EmployeesController(IService<Employee> employeeService, IService<Department> departmentService, IService<Manager> managerService)
        {
            _Employee = employeeService;
            _Department = departmentService;
            _Manager = managerService;
        }

        public async Task<IActionResult> Index()
        {

            var employees = await _Employee.GetAllAsync();
            if (employees != null)
            {
                var department = await _Department.GetAllAsync();
                ViewBag.Department = department;
                var manager = await _Manager.GetAllAsync();
                ViewBag.Manager = manager;
            }
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
            var department = _Department.GetAllAsync().Result;
            ViewData["Department"] = new SelectList(department, "Id", "Name");
            var manager = _Manager.GetAllAsync().Result;
            ViewData["Manager"] = new SelectList(manager, "Id", "Name");
            return View();
        }

        // POST: employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employees)
        {
            if (ModelState.IsValid)
            {
                // Check if a file has been uploaded
                if (employees.ImageURL != null)
                {
                    // Specify the folder where images will be stored
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    // Ensure the folder exists
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Generate a unique filename to prevent conflicts
                    var uniqueFileName = employees.Name + "_" + employees.ImageURL.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save the uploaded file to the specified path
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await employees.ImageURL.CopyToAsync(stream);
                    }

                    // Save the relative path of the image in the database
                    employees.ImagePath = "/images/" + uniqueFileName;
                }

                // Add the employee to the database
                await _Employee.AddAsync(employees);


                return RedirectToAction(nameof(Index));
            }

            // Return the view with validation errors if the model is invalid
            return View(employees);
        }


        // GET: employees/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var department = _Department.GetAllAsync().Result;
            ViewData["Department"] = new SelectList(department, "Id", "Name");
            var manager = _Manager.GetAllAsync().Result;
            ViewData["Manager"] = new SelectList(manager, "Id", "Name");
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

