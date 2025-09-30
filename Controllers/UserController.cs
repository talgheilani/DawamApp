using DawamApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace DawamApp.Controllers
{
    public class UserController : Controller
    {
        private readonly DawamAppDbContext _context;

        public UserController(DawamAppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Statuses = await _context.EmployeeStatuses
                .OrderByDescending(s => s.StartDate)
                .ToListAsync();

            return View(new EmployeeStatus());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(EmployeeStatus status)
        {
            if (ModelState.IsValid)
            {
                if (status.EndDate < status.StartDate)
                {
                    ModelState.AddModelError("EndDate", "End Date cannot be earlier than Start Date");
                }
                else
                {
                    _context.EmployeeStatuses.Add(status);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewBag.Statuses = await _context.EmployeeStatuses
                .OrderByDescending(s => s.StartDate)
                .ToListAsync();

            return View(status);
        }
    }
}
