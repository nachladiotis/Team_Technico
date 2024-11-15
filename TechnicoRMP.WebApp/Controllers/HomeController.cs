using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using TechnicoRMP.WebApp.Models;

namespace TechnicoRMP.WebApp.Controllers
{
    public class HomeController : Controller
    {
        // Μέθοδος που επιστρέφει την αρχική σελίδα του χρήστη
        public IActionResult Index()
        {     
            return View();
        }

    }
}
