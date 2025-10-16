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

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
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

                _context.UserAccounts.Add(model);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Account created successfully! Please login.";
                return RedirectToAction("Login");
            }

            return View(model);
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
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
                // Email not registered
                ViewBag.NotRegistered = true;
                ViewBag.Error = "No account found with this email. Please create one.";
                return View();
            }

            if (user.Password != Password)
            {
                // Wrong password
                ViewBag.WrongPassword = true;
                ViewBag.Error = "Incorrect password. Please try again.";
                return View();
            }

            // Successful login → store session
            HttpContext.Session.SetString("FirstName", user.FirstName);
            HttpContext.Session.SetString("LastName", user.LastName);
            HttpContext.Session.SetString("Email", user.Email);

            return RedirectToAction("Index", "Home");
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
