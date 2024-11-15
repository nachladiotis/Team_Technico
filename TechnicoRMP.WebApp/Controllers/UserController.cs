namespace TechnicoRMP.WebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Reflection;
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

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            HttpResponseMessage response = await _client.GetAsync($"{_client.BaseAddress}/User/{id}");
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
                        //TypeOfUser = user.TypeOfUser,
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
        StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _client.PutAsync(_client.BaseAddress + "/User/PutAsync", content);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("GetProfile", new { updatedUser.Id });
        }
        return View(new UserProfileViewModelUpdate());
    }

    //Soft Delete
    [HttpPost]
    public async Task<IActionResult> SoftDelete([FromRoute] int id)
    {
        try
        {
            HttpResponseMessage response = await _client.PostAsync($"User/SoftDelete/{id}", null);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index"); 
            }
            else
            {
                string errorDetails = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", "Error deleting user. Please try again.");
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "An unexpected error occurred.");
        }

        return RedirectToAction("GetProfile", new { id });
    }

}
