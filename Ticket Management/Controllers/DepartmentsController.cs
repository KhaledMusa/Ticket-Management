using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Ticket_Management.Models;
using Ticket_Management.Services;

namespace Ticket_Management.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IService<Department> _Depatment;

        public DepartmentsController(IService<Department> departmentService)
        {
            _Depatment = departmentService;
        }

        // GET: Department
        public async Task<IActionResult> Index()
        {
            var departments = await _Depatment.GetAllAsync();
            return View(departments);
        }

        // GET: Department/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var department = await _Depatment.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        // GET: Department/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)
            {
                await _Depatment.AddAsync(department);
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // GET: Department/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var department = await _Depatment.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        // POST: Department/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Department department)
        {
            if (id != department.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _Depatment.UpdateAsync(department);
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // GET: Department/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _Depatment.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        // POST: Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _Depatment.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
