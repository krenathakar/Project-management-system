using Microsoft.AspNetCore.Mvc;
using PMS.Data;
using PMS.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace PMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [HttpPost]
public IActionResult Login(LoginViewModel model)
{
    if (ModelState.IsValid)
    {
        var user = _context.User.FirstOrDefault(u => u.Email == model.Email);
        if (user != null)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
            if (result == PasswordVerificationResult.Success)
            {
                TempData["Success"] = "Login successful!";

                // ✅ Redirect based on role
                if (model.Role == "Admin")
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                else if (model.Role == "User")
                {
                    return RedirectToAction("Dashboard", "User");
                }
            }
        }
        ModelState.AddModelError("", "Invalid login attempt");
    }
    return View(model);
}

    }
}

