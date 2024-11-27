using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ticket_Management.Models;
using Ticket_Management.Services;

namespace Ticket_Management.Controllers
{
    public class ManagersController : Controller
    {

        private readonly IService<Manager> _Manager;
        private readonly IService<Department> _Department;

        public ManagersController(IService<Manager> managerService,IService<Department> departmentService)
        {
            _Manager = managerService;
            _Department = departmentService;
        }
   

        // GET: managers
        public async Task<IActionResult> Index()
        {
            var depatment = _Department.GetAllAsync();
            ViewBag.Department=depatment.Result;
            var managers = await _Manager.GetAllAsync();
            return View(managers);
        }

        // GET: managers/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var managers = await _Manager.GetByIdAsync(id);
            if (managers == null)
            {
                return NotFound();
            }
            return View(managers);
        }

        // GET: managers/Create
        public IActionResult Create()
        {
            var department=_Department.GetAllAsync().Result;
            ViewData["Department"] = new SelectList(department,"Id","Name");

            return View();
        }

        // POST: managers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Manager managers)
        {
            var department = _Department.GetAllAsync().Result;
            ViewData["Department"] = new SelectList(department, "Id", "Name");
            if (ModelState.IsValid)
            {
                await _Manager.AddAsync(managers);
                return RedirectToAction(nameof(Index));
            }
            return View(managers);
        }

        // GET: managers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var managers = await _Manager.GetByIdAsync(id);
            if (managers == null)
            {
                return NotFound();
            }
            return View(managers);
        }

        // POST: managers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Manager managers)
        {
            if (id != managers.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _Manager.UpdateAsync(managers);
                return RedirectToAction(nameof(Index));
            }
            return View(managers);
        }

        // GET: managers/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var managers = await _Manager.GetByIdAsync(id);
            if (managers == null)
            {
                return NotFound();
            }
            return View(managers);
        }

        // POST: managers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _Manager.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
