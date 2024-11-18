using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;
using TechnicoRMP.WebApp;
using TechnicoRMP.WebApp.Models;

public class AccountController(IHttpClientFactory httpClientFactory) : Controller
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Logout()
    {
        ActiveUser.SetUser(null);
        var cookie = Request.Cookies.Keys.FirstOrDefault(s=> s.Contains("LoggedInUser"));
        if (!string.IsNullOrEmpty(cookie))
        {
            Response.Cookies.Delete(cookie);
        }
        
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        if (!IsValidPassword(model.Password))
        {
            ModelState.AddModelError("Password", "Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, and one number.");
            return View(model);
        }

        var client = _httpClientFactory.CreateClient("ApiClient");
        var uri = new Uri($"{client.BaseAddress}/Auth/register");

        if(model.Password != model.ConfirmPassword)
        {
            ModelState.AddModelError("Password", "Passwords doesnt match");
           
            return View(model);
        }

        var dto = new CreateUserRequest
         { Email = model.Email,
          Name = model.Name,
          Password = model.Password,
          Surname = model.Surname,
          VatNumber = model.VatNumber,
          Address = model.Address,
          PhoneNumber = model.PhoneNumber,
         };

        var response = await client.PostAsJsonAsync(uri, dto);

        var result = await response.Content.ReadFromJsonAsync<Result>();

        if (response.IsSuccessStatusCode && result?.Status == 1)
        {
            return RedirectToAction("Login", "Account"); 
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
            return View(model);
        }
    }

    [HttpGet]
    public IActionResult Login()
    {
        if(ActiveUser.User is null)
            return View();
     
        return null!;
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {

        if (!ModelState.IsValid)
        {
            return View(loginViewModel);
        }

        var client = _httpClientFactory.CreateClient("ApiClient");

        var dto = new LoginDto { Email = loginViewModel.Email, Password = loginViewModel.Password };
        var uri = new Uri($"{client.BaseAddress}/Auth/login");

        var response = await client.PostAsJsonAsync(uri, dto);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<UserLoginResponse>>();

            if (result != null && result.Status > 0)
            {
                var options = new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(result.Value!.SesionExpirationInDays),
                        HttpOnly = true,
                        Secure = true
                    };
                    var loggedInUser = JsonSerializer.Serialize<UserDto>(result.Value!.UserDto);

                    Response.Cookies.Append("LoggedInUser", loggedInUser, options);

                    ActiveUser.SetUser(result.Value!.UserDto);
                    return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["ErrorMessage"] = result?.Message ?? "Error while login";
            }
        }
        else
        {
            ViewData["ErrorMessage"] = "Internal Server error";
        }

        return View(loginViewModel);
    }

    private bool IsValidPassword(string password)
    {
        if (password.Length < 8)
        {
            return false;
        }
        if (!password.Any(char.IsUpper))
        {
            return false;
        }
        if (!password.Any(char.IsLower))
        {
            return false;
        }
        if (!password.Any(char.IsDigit))
        {
            return false;
        }
        return true;
    }
}
