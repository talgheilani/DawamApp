using DawamApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DawamApp.Controllers
{
    public class UserController : Controller
    {
        private readonly DawamAppDbContext _context;

        public UserController(DawamAppDbContext context)
        {
            _context = context;
        }

        // GET: User/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeStatus model, string StartDateStr, string EndDateStr)
        {
            string[] dateFormats = { "dd/MM/yyyy", "yyyy-MM-dd" };

            if (!string.IsNullOrEmpty(StartDateStr) &&
                DateTime.TryParseExact(StartDateStr, dateFormats, null,
                System.Globalization.DateTimeStyles.None, out DateTime start))
            {
                model.StartDate = start;
            }
            else ModelState.AddModelError("StartDate", "Invalid Start Date format");

            if (!string.IsNullOrEmpty(EndDateStr) &&
                DateTime.TryParseExact(EndDateStr, dateFormats, null,
                System.Globalization.DateTimeStyles.None, out DateTime end))
            {
                model.EndDate = end;
            }
            else ModelState.AddModelError("EndDate", "Invalid End Date format");

            if (ModelState.IsValid)
            {
                _context.EmployeeStatuses.Add(model);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Status saved successfully!";
                return RedirectToAction(nameof(Create));
            }

            return View(model);
        }

        // GET: User/Edit (list all)
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var statuses = await _context.EmployeeStatuses.ToListAsync();
            return View(statuses);
        }

        // GET: User/EditSingle/5
        [HttpGet]
        public async Task<IActionResult> EditSingle(int id)
        {
            var status = await _context.EmployeeStatuses.FindAsync(id);
            if (status == null) return NotFound();
            return View(status);
        }

        // POST: User/EditSingle/5
        [HttpPost]
        public async Task<IActionResult> EditSingle(int id, EmployeeStatus model, string StartDateStr, string EndDateStr)
        {
            if (id != model.Id) return NotFound();

            string[] dateFormats = { "dd/MM/yyyy", "yyyy-MM-dd" };

            if (!string.IsNullOrEmpty(StartDateStr) &&
                DateTime.TryParseExact(StartDateStr, dateFormats, null,
                System.Globalization.DateTimeStyles.None, out DateTime start))
            {
                model.StartDate = start;
            }
            else ModelState.AddModelError("StartDate", "Invalid Start Date format");

            if (!string.IsNullOrEmpty(EndDateStr) &&
                DateTime.TryParseExact(EndDateStr, dateFormats, null,
                System.Globalization.DateTimeStyles.None, out DateTime end))
            {
                model.EndDate = end;
            }
            else ModelState.AddModelError("EndDate", "Invalid End Date format");

            if (ModelState.IsValid)
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Status updated successfully!";
                return RedirectToAction(nameof(Edit));
            }

            return View(model);
        }

        // POST: User/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _context.EmployeeStatuses.FindAsync(id);
            if (status != null)
            {
                _context.EmployeeStatuses.Remove(status);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Status deleted successfully!";
            }
            return RedirectToAction(nameof(Edit));
        }
    }
}
