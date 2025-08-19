using Microsoft.AspNetCore.Mvc;
using PMS.Data;
using PMS.Models;

namespace PMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Register
        public IActionResult Register()
        {
            return View();
        }

        // GET: Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.User
                    .FirstOrDefault(u => u.Email == model.Email && u.PasswordHash == model.Password);

                if (user != null)
                {
                    // ✅ Login success → redirect based on role
                    if (user.Role == "Admin")
                        return RedirectToAction("Dashboard", "Admin");
                    else
                        return RedirectToAction("Dashboard", "User");
                }

                ModelState.AddModelError("", "Invalid Email or Password");
            }

            return View(model);
        }
    }
}
