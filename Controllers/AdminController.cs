using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Models;
using PMS.ViewModels;

namespace PMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _ctx;
        private readonly IPasswordHasher<AppUser> _hasher;

        public AdminController(AppDbContext ctx, IPasswordHasher<AppUser> hasher)
        {
            _ctx = ctx;
            _hasher = hasher;
        }

        // LIST
        public async Task<IActionResult> Index()
        {
            var users = await _ctx.AppUsers.OrderByDescending(u => u.CreatedAt).ToListAsync();
            return View(users);

             return View();
             
             ViewData["HideNavbar"] = true; 
                return View();
        }

        // CREATE - GET
        public IActionResult Create()
        {
            return PartialView("_CreateUser");
        }

        // CREATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminCreateUserVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var email = vm.Email.Trim().ToLower();
            if (await _ctx.AppUsers.AnyAsync(u => u.Email == email))
            {
                ModelState.AddModelError(nameof(vm.Email), "Email already exists.");
                return View(vm);
            }
            

            var user = new AppUser
            {
                FullName = vm.FullName.Trim(),
                Email = email,
                Role = vm.Role,
                Gender = vm.Gender,
                Address = vm.Address,
                ContactNo = vm.ContactNo
            };

            user.PasswordHash = _hasher.HashPassword(user, vm.Password);

            _ctx.AppUsers.Add(user);
            await _ctx.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        

public async Task<IActionResult> Edit(int id)
{
    var user = await _ctx.AppUsers.FindAsync(id);
    if (user == null) return NotFound();

    var vm = new AdminEditUserVM
    {
        Id = user.Id,
        FullName = user.FullName,
        Email = user.Email,
        Role = user.Role,
        Gender = user.Gender,
        Address = user.Address,
        ContactNo = user.ContactNo
    };
    return PartialView("_EditUser", vm);
}


        // EDIT - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AdminEditUserVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = await _ctx.AppUsers.FindAsync(vm.Id);
            if (user == null) return NotFound();

            // update allowed fields (do not change password here)
            user.FullName = vm.FullName.Trim();
            user.Email = vm.Email.Trim().ToLower();
            user.Role = vm.Role;
            user.Gender = vm.Gender;
            user.Address = vm.Address;
            user.ContactNo = vm.ContactNo;

            await _ctx.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // DELETE - GET (confirmation)
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _ctx.AppUsers.FindAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        // DELETE - POST (confirmed)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _ctx.AppUsers.FindAsync(id);
            if (user != null)
            {
                _ctx.AppUsers.Remove(user);
                await _ctx.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
    
       