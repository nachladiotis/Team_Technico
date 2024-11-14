namespace TechnicoRMP.WebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using TechnicoRMP.Models;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;
using TechnicoRMP.WebApp.Models;

public class UserController(IHttpClientFactory httpClientFactory) : Controller
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    [HttpGet]
    public IActionResult UserProfile()
    {
        return View();
    }

    //GetProfile
    [HttpGet]
    public async Task<IActionResult> GetProfile(int id)
    {
        var client = _httpClientFactory.CreateClient("ApiClient");
        var uri = new Uri($"{client.BaseAddress}/Auth/user/{id}");
        var response = await client.GetAsync(uri);
        if (response.IsSuccessStatusCode)
        {
            var user = await response.Content.ReadFromJsonAsync<CreateUserResponse>();
            return View(user);
            //return RedirectToAction("GetProfile", "User");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Unable to retrieve user data.");
        }

        return View(); 
    }

    //UpdateProfile
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateProfile(UpdateUserRequest userProfile)
    {
        var client = _httpClientFactory.CreateClient("ApiClient");
        var uri = new Uri($"{client.BaseAddress}/Auth/update");
        var response = await client.PutAsJsonAsync(uri, userProfile);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("UpdateProfile", new { id = userProfile.Id });
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Update Failed");
            return View(userProfile);
        }
    }

    //Soft Delete
    [HttpPost]
    public async Task<IActionResult> SoftDeleteProfile(int id)
    {
        var client = _httpClientFactory.CreateClient("ApiClient");
        var uri = new Uri($"{client.BaseAddress}/Auth/softdelete/{id}");

        var response = await client.DeleteAsync(uri);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index", "Home");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Soft Delete failed");
            return RedirectToAction("EditProfile", new { id = id });
        }
    }
}
