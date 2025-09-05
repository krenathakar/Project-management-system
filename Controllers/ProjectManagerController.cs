using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PMS.Controllers
{
    [Authorize(Roles = "ProjectManager")]
    public class ProjectManagerController : Controller
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
