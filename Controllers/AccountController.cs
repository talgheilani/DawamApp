using DawamApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace DawamApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly DawamAppDbContext _context;

        public AccountController(DawamAppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(UserAccount model, string ConfirmPassword)
        {
            if (ModelState.IsValid)
            {
                if (model.Password != ConfirmPassword)
                {
                    ModelState.AddModelError("Password", "Passwords do not match");
                    return View(model);
                }

                var existingUser = _context.UserAccounts.FirstOrDefault(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email already registered");
                    return View(model);
                }

                // Save user
                _context.UserAccounts.Add(model);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Account created successfully! Please login.";
                return RedirectToAction("Login");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string Email, string Password)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ViewBag.Error = "Email and Password are required";
                return View();
            }

            var user = _context.UserAccounts.FirstOrDefault(u => u.Email == Email);

            if (user == null)
            {
                ViewBag.Error = "No account found with this email. Please create one.";
                return View();
            }

            if (user.Password != Password)
            {
                ViewBag.Error = "Incorrect password. Please try again.";
                return View();
            }

            // Assign role manually for your email
            if(user.Email.ToLower() == "tasnim.k.algheilani@mem.gov.om")
                user.Role = "Admin";

            // Store session
            HttpContext.Session.SetString("FirstName", user.FirstName);
            HttpContext.Session.SetString("LastName", user.LastName);
            HttpContext.Session.SetString("Email", user.Email);
            HttpContext.Session.SetString("Role", user.Role);

            return RedirectToAction("Index", "home"); // Redirect to dashboard
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
