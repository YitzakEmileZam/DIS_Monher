using System.Diagnostics;
using DIS_Monher.Models;
using Microsoft.AspNetCore.Mvc;

namespace DIS_Monher.Controllers
{
    public class HomeController : Controller
    {
        // Cada rol aterriza en su propia vista
        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
                return RedirectToAction("Index", "Admin");

            return RedirectToAction("Index", "Catalogo");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
