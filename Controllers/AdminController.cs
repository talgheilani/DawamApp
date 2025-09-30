using DawamApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DawamApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly DawamAppDbContext _context;

        public AdminController(DawamAppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetEvents(string statuses)
        {
            var selectedStatuses = string.IsNullOrEmpty(statuses)
                ? new List<StatusType>()
                : statuses.Split(',').Select(s => Enum.Parse<StatusType>(s)).ToList();

            var query = _context.EmployeeStatuses.AsQueryable();

            if (selectedStatuses.Any())
                query = query.Where(s => selectedStatuses.Contains(s.Status));

            var list = query.ToList();

            var events = list.Select(s => new
            {
                title = $"{s.EmployeeName} - {s.Status}",
                start = s.StartDate.ToString("yyyy-MM-dd"),
                end = s.EndDate.AddDays(1).ToString("yyyy-MM-dd"), // FullCalendar exclusive end
                color = s.Status switch
                {
                    StatusType.Training => "#28a745",
                    StatusType.Discharge => "#dc3545",
                    StatusType.Vacation => "#ffc107",
                    StatusType.Errand => "#0d6efd",
                    StatusType.Workshop => "#6f42c1",
                    _ => "#6c757d"
                }
            }).ToList();

            return Json(events);
        }
    }
}
