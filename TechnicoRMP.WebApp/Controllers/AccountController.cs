using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;
using TechnicoRMP.WebApp.Models;

public class AccountController(IHttpClientFactory httpClientFactory) : Controller
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var client = _httpClientFactory.CreateClient("ApiClient");
        var uri = new Uri($"{client.BaseAddress}/Auth/register");
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

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Login", "Account"); // Redirect to login or a success page
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
        return View(); // This will render Views/Account/Login.cshtml
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        var client = _httpClientFactory.CreateClient("ApiClient");

        // Prepare the login data to be sent to your API
        var dto = new LoginDto { Email = loginViewModel.Email, Password = loginViewModel.Password };

        var uri = new Uri($"{client.BaseAddress}/Auth/login");

        var response = await client.PostAsJsonAsync(uri,dto);

       // var response = await client.PostAsJsonAsync("/Auth/login",string.Empty );

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<CreateUserResponse>>();
            // Handle a successful login
        }
        else
        {
            // Handle login failure
        }

        return View();
    }
}
