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

            //Checks if userSesion on cookeis is alive and if a user is set to the provider class
            //We dont want to desirialize object all the time on every redirection to Home 
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
