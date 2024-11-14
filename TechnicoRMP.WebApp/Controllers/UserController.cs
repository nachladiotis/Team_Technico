namespace TechnicoRMP.WebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using TechnicoRMP.Models;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;
using TechnicoRMP.WebApp.Models;

public class UserController : Controller
{
    Uri baseAdsress = new Uri("https://localhost:7038/api");
    private readonly HttpClient _client;

    public UserController()
    {
        _client = new HttpClient();
        _client.BaseAddress = baseAdsress;
    }


    [HttpGet("User/GetProfile/{id}")]
    public async Task<IActionResult> GetProfile(int id)
    {
        UserProfileViewModel user = new UserProfileViewModel();
        HttpResponseMessage response = await _client.GetAsync($"{_client.BaseAddress}/User/{id}");
        if (response.IsSuccessStatusCode)
        {
            string data = await response.Content.ReadAsStringAsync();
            Content(data, "application/json");
            user = JsonConvert.DeserializeObject<UserProfileViewModel>(data);
        }
        else
        {
            ModelState.AddModelError("", "Unable to load user data.");
        }
        return View(user);
    }

    [HttpPut("User/UpdateProfile/{id}")]
    public async Task<IActionResult> UpdateProfile(UserProfileViewModel updatedUser)
    {
        if (!ModelState.IsValid)
        {
            return View(updatedUser);
        }

        var content = new StringContent(JsonConvert.SerializeObject(updatedUser), Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _client.PutAsync($"{_client.BaseAddress}/User/{updatedUser.Id}", content);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("GetProfile", new { id = updatedUser.Id }); 
        }
        else
        {
            ModelState.AddModelError("", "Error updating profile. Please try again.");
            return View(updatedUser); 
        }
    }


    //    //UpdateProfile
    //    [HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> UpdateProfile(UpdateUserRequest userProfile)
    //{
    //    var client = _httpClientFactory.CreateClient("ApiClient");
    //    var uri = new Uri($"{client.BaseAddress}/Auth/update");
    //    var response = await client.PutAsJsonAsync(uri, userProfile);
    //    if (response.IsSuccessStatusCode)
    //    {
    //        return RedirectToAction("UpdateProfile", new { id = userProfile.Id });
    //    }
    //    else
    //    {
    //        ModelState.AddModelError(string.Empty, "Update Failed");
    //        return View(userProfile);
    //    }
    //}

    ////Soft Delete
    //[HttpPost]
    //public async Task<IActionResult> SoftDeleteProfile(int id)
    //{
    //    var client = _httpClientFactory.CreateClient("ApiClient");
    //    var uri = new Uri($"{client.BaseAddress}/Auth/softdelete/{id}");

    //    var response = await client.DeleteAsync(uri);

    //    if (response.IsSuccessStatusCode)
    //    {
    //        return RedirectToAction("Index", "Home");
    //    }
    //    else
    //    {
    //        ModelState.AddModelError(string.Empty, "Soft Delete failed");
    //        return RedirectToAction("EditProfile", new { id = id });
    //    }
    //}
}
