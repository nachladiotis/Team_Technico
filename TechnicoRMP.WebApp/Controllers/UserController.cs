namespace TechnicoRMP.WebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using TechnicoRMP.Shared.Dtos;
using TechnicoRMP.WebApp.Models;

public class UserController(IHttpClientFactory httpClientFactory) : Controller
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    [HttpGet("User/GetProfile/{id}")]
    public async Task<IActionResult> GetProfile(int id)
    {
        UserProfileViewModel user = new UserProfileViewModel();
        var client = _httpClientFactory.CreateClient("ApiClient");
        var uri = new Uri($"{client.BaseAddress}/User/{id}");
        HttpResponseMessage response = await client.GetAsync(uri);
        if (response.IsSuccessStatusCode)
        {
            string data = await response.Content.ReadAsStringAsync();
            Content(data, "application/json");
            user = JsonConvert.DeserializeObject<UserProfileViewModel>(data)!;
        }
        else
        {
            ModelState.AddModelError("", "Unable to load user data.");
        }
        return View(user);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            HttpResponseMessage response = await client.GetAsync($"{client.BaseAddress}/User/{id}");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<CreateUserRequest>(data);
                if (user != null)
                {
                    var viewmodel = new UserProfileViewModelUpdate
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Surname = user.Surname,
                        VatNumber = user.VatNumber,
                        Address = user.Address,
                        PhoneNumber = user.PhoneNumber,
                        Email = user.Email,
                        Password = user.Password
                    };
                    return View(viewmodel);
                }
            }
            return View(new UserProfileViewModelUpdate
            {
                Id = id
            });
        }
        catch (Exception)
        {
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UserProfileViewModelUpdate updatedUser)
    {
        string data = JsonConvert.SerializeObject(updatedUser);
        var client = _httpClientFactory.CreateClient("ApiClient");
        StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PutAsync(client.BaseAddress + "/User/PutAsync", content);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("GetProfile", new { updatedUser.Id });
        }
        return View(new UserProfileViewModelUpdate());
    }

    [HttpGet]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/User/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<IsActiveRequest>(data);
                var viewmodel = new IsActiveViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    IsActive = user.IsActive
                };
                return View(viewmodel);
            }
            return View(new IsActiveViewModel
            {
                Id = id
            });
        }
        catch (Exception ex)
        {
            return View();
        }
    }

    [HttpGet]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            HttpResponseMessage response = await client.PostAsync(
                $"{client.BaseAddress}/User/SoftDelete/{id}",
                null);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "User was successfully deactivated.";
                return RedirectToAction("Logout", "Account");
            }
            else
            {
                string errorDetails = await response.Content.ReadAsStringAsync();
                TempData["ErrorMessage"] = $"Failed to deactivate the user: {errorDetails}";
            }
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "An unexpected error occurred while deactivating the user.";
        }
        return RedirectToAction("GetProfile");
    }

}






