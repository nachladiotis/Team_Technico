using Microsoft.AspNetCore.Mvc;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;
using TechnicoRMP.WebApp.Models;

public class AccountController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AccountController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
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
            
        }
        else
        {
            // Handle login failure
        }
        return View();
    }
}
