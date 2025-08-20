using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using PMS.Data;
using PMS.Models;
using PMS.ViewModels;

namespace PMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _ctx;
        private readonly IPasswordHasher<AppUser> _hasher;

        public AccountController(AppDbContext ctx, IPasswordHasher<AppUser> hasher)
        {
            _ctx = ctx;
            _hasher = hasher;
        }

        // ---------- Register ----------
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var email = model.Email.Trim().ToLower();
            if (await _ctx.Users.AnyAsync(u => u.Email == email))
            {
                ModelState.AddModelError(nameof(model.Email), "Email already registered.");
                return View(model);
            }

            var user = new AppUser
            {
                FullName = model.FullName.Trim(),
                Email = email,
                Role = model.Role
            };

            user.PasswordHash = _hasher.HashPassword(user, model.Password);

            _ctx.Users.Add(user);
            await _ctx.SaveChangesAsync();

            TempData["msg"] = "Registration successful. Please sign in.";
            return RedirectToAction(nameof(Login));
        }

        // ---------- Login ----------
        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.Message = TempData["msg"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model, string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View(model);

            var email = model.Email.Trim().ToLower();
            var user = await _ctx.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null || user.Role != model.Role)
            {
                ModelState.AddModelError("", "Invalid email/role or password.");
                return View(model);
            }

            var verify = _hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
            if (verify == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "Invalid email/role or password.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            // Redirect to role dashboards
            return user.Role == Role.Admin
                ? RedirectToAction("Index", "Admin")
                : RedirectToAction("Index", "User");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        public IActionResult AccessDenied() => View();
    }
}
