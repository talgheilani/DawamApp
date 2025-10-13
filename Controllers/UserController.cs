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
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeStatus model, string StartDateStr, string EndDateStr)
        {
            if (!string.IsNullOrEmpty(StartDateStr))
            {
                if (DateTime.TryParseExact(StartDateStr, "dd/MM/yyyy", null,
                    System.Globalization.DateTimeStyles.None, out DateTime start))
                {
                    model.StartDate = start;
                }
                else ModelState.AddModelError("StartDate", "Invalid Start Date format");
            }

            if (!string.IsNullOrEmpty(EndDateStr))
            {
                if (DateTime.TryParseExact(EndDateStr, "dd/MM/yyyy", null,
                    System.Globalization.DateTimeStyles.None, out DateTime end))
                {
                    model.EndDate = end;
                }
                else ModelState.AddModelError("EndDate", "Invalid End Date format");
            }

            if (ModelState.IsValid)
            {
                _context.EmployeeStatuses.Add(model);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Status saved successfully!";
                return RedirectToAction(nameof(Create));
            }

            return View(model);
        }

        // GET: User/Edit (table listing)
        [HttpGet]
        [Route("User/Edit")]
        public async Task<IActionResult> Edit()
        {
            var statuses = await _context.EmployeeStatuses.ToListAsync();
            return View(statuses); // Edit.cshtml
        }

        // GET: User/Edit/5 (edit single)
        [HttpGet]
        [Route("User/Edit/{id:int}")]
        public async Task<IActionResult> EditSingle(int id)
        {
            var status = await _context.EmployeeStatuses.FindAsync(id);
            if (status == null) return NotFound();
            return View("EditSingle", status);
        }

        // POST: User/Edit/5
        [HttpPost]
        [Route("User/Edit/{id:int}")]
        public async Task<IActionResult> EditSingle(int id, EmployeeStatus model, string StartDateStr, string EndDateStr)
        {
            if (id != model.Id) return NotFound();

            DateTime start = model.StartDate;
            DateTime end = model.EndDate;

            if (!string.IsNullOrEmpty(StartDateStr))
            {
                if (DateTime.TryParseExact(StartDateStr, "dd/MM/yyyy", null,
                    System.Globalization.DateTimeStyles.None, out DateTime parsedStart))
                {
                    start = parsedStart;
                }
                else ModelState.AddModelError("StartDate", "Invalid Start Date format");
            }

            if (!string.IsNullOrEmpty(EndDateStr))
            {
                if (DateTime.TryParseExact(EndDateStr, "dd/MM/yyyy", null,
                    System.Globalization.DateTimeStyles.None, out DateTime parsedEnd))
                {
                    end = parsedEnd;
                }
                else ModelState.AddModelError("EndDate", "Invalid End Date format");
            }

            model.StartDate = start;
            model.EndDate = end;

            if (ModelState.IsValid)
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Status updated successfully!";
                return RedirectToAction(nameof(Edit));
            }

            return View("EditSingle", model);
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
