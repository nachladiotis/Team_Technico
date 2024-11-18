using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TechnicoRMP.Shared.Dtos;

namespace TechnicoRMP.WebApp.Controllers
{
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            var userDtoString = Request.Cookies["LoggedInUser"];

            if (!string.IsNullOrEmpty(userDtoString) && ActiveUser.User is null)
            {
                var userDto = JsonSerializer.Deserialize<UserDto>(userDtoString);
                ActiveUser.SetUser(userDto);
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
