using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IActionResult Index() 
        {
            ViewBag.Message = TempData["msg"];
            return View();

            ViewData["HideNavbar"] = true; // Hides navbar
            return View();
        }
        
    }
}
