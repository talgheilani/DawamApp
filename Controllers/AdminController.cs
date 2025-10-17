using DawamApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DawamApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly DawamAppDbContext _context;
        private const string AdminEmail = "tasnim.k.algheilani@mem.gov.om";

        public AdminController(DawamAppDbContext context)
        {
            _context = context;
        }

        // Dashboard
        public IActionResult Index()
        {
            var email = HttpContext.Session.GetString("Email");
            ViewBag.AccessGranted = email == AdminEmail; // Admin flag
            return View();
        }

        // Get events for FullCalendar
        [HttpGet]
        public IActionResult GetEvents(string statuses)
        {
            var email = HttpContext.Session.GetString("Email");
            bool isAdmin = email == AdminEmail;

            var selectedStatuses = string.IsNullOrEmpty(statuses)
                ? new List<StatusType>()
                : statuses.Split(',').Select(s => Enum.Parse<StatusType>(s)).ToList();

            // Query
            var query = _context.EmployeeStatuses.AsQueryable();

            if (!isAdmin)
            {
                // Normal users see only their own records
                var firstName = HttpContext.Session.GetString("FirstName");
                var lastName = HttpContext.Session.GetString("LastName");
                string fullName = $"{firstName} {lastName}";
                query = query.Where(s => s.EmployeeName == fullName);
            }

            if (selectedStatuses.Any())
            {
                query = query.Where(s => selectedStatuses.Contains(s.Status));
            }

            var list = query.ToList();

            var events = list.Select(s => new
            {
                title = $"{s.EmployeeName} - {s.Status}",
                start = s.StartDate.ToString("yyyy-MM-dd"),
                end = s.EndDate.AddDays(1).ToString("yyyy-MM-dd"),
                color = s.Status switch
                {
                    StatusType.Training => "#7203a1ff",
                    StatusType.Discharge => "#04c487ff",
                    StatusType.Holiday => "#d7275fff",
                    StatusType.Duty => "#fcb54aff",
                    StatusType.Workshop => "#0487c4ff",
                    _ => "#6c757d"
                }
            }).ToList();

            return Json(events);
        }
    }
}
