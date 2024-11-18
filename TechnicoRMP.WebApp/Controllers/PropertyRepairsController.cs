using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;
using TechnicoRMP.WebApp.Models;
using TechnicoRMP.Models;
using System.Diagnostics;
using System;

namespace TechnicoRMP.WebApp.Controllers;

public class PropertyRepairsController(IHttpClientFactory httpClientFactory) : Controller
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    [HttpGet]
    public async Task<IActionResult> GetAll()

    {
        var client = _httpClientFactory.CreateClient("ApiClient");
        var uri = new Uri($"{client.BaseAddress}/Repair");

        var response = await client.GetAsync(uri);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<List<PropertyRepairResponseDTO>>();
            var jsonData = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<PropertyRepairResponseDTO>>(jsonData);

            if (data == null) return View(new List<PropertyRepairViewModel>());
            

            List<PropertyRepairViewModel> list = new List<PropertyRepairViewModel>();
            foreach (var property in data)
            {
                var listitem = new PropertyRepairViewModel
                {
                    Address = property.Address,
                    Cost = property.Cost,
                    RepairStatus = property.RepairStatus,
                    TypeOfRepair = property.TypeOfRepair,
                    Id = property.Id,
                    Date = property.Date
                };
                list.Add(listitem);
            }
            return View(list);
        }
        else
        {
            return View(new List<PropertyRepairViewModel>());
        }
    }


    // GET : PropertyRepairs/user/id
    [HttpGet("Repair/GetRepairsByUserId/{id}")]
    public async Task<IActionResult> GetRepairsByUserId(long id)
    {
        var client = _httpClientFactory.CreateClient("ApiClient");
        var uri = new Uri($"{client.BaseAddress}/Repair/user/{id}");

        var response = await client.GetAsync(uri);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<List<PropertyRepairResponseDTO>>();

            if (result == null || result.Count == 0)
            {
                return View(new List<PropertyRepairViewModel>());
            }

            List<PropertyRepairViewModel> list = new List<PropertyRepairViewModel>();
            foreach (var property in result)
            {
                var listitem = new PropertyRepairViewModel
                {
                    Address = property.Address,
                    Cost = property.Cost,
                    RepairStatus = property.RepairStatus,
                    TypeOfRepair = property.TypeOfRepair,
                    Id = property.Id,
                    Date = property.Date
                };
                list.Add(listitem);
            }
            return View(list);
        }
        else
        {
            return View(new List<PropertyRepairViewModel>());
        }
    }
    // GET: PropertyRepairs/GetById/1
    [HttpGet]
    public async Task<IActionResult> GetById(long? id)
    {

        var client = _httpClientFactory.CreateClient("ApiClient");
        var uri = new Uri($"{client.BaseAddress}/Repair/{id}");

        var response = await client.GetAsync(uri);
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<PropertyRepairResponseDTO>(jsonData);
            if (data == null)
            {
                return View(new List<PropertyRepairViewModel>());
            }
            var repairbyid = new PropertyRepairViewModel
            {
                Address = data.Address,
                Cost = data.Cost,
                RepairStatus = data.RepairStatus,
                TypeOfRepair = data.TypeOfRepair,
                Id = data.Id,
                Date = data.Date
            };
            return View(new List<PropertyRepairViewModel> { repairbyid });
        }
        else
        {
            return View(new List<PropertyRepairViewModel>());
        }
    }

    // GET: PropertyRepairs/Create
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var propertyItems = new List<PropertyItemViewModel>();

        var userId = ActiveUser.User.Id;

        var client = _httpClientFactory.CreateClient("ApiClient");
        var requestUri = new Uri($"{client.BaseAddress}/PropertyItem/GetPropertyItemByUserId/{userId}");

        HttpResponseMessage response = await client.GetAsync(requestUri);

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            var apiResult = JsonConvert.DeserializeObject<Result<PropertyItemsByUserDto>>(data);

            if (apiResult?.Status == 0 && apiResult.Value?.PropertyItems != null)
            {
                propertyItems = apiResult.Value.PropertyItems.Select(item => new PropertyItemViewModel
                {
                    Id = item.Id,
                    Address = item.Address,
                    E9Number = item.E9Number
                }).ToList();
            }
        }
        ViewBag.PropertyItems = propertyItems;

        return View();
    }

    // POST: PropertyRepairs/Create
    [HttpPost]
    public async Task<IActionResult> Create(PropertyRepairViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var uri = new Uri($"{client.BaseAddress}/Repair");

            var propertyRepair = new CreatePropertyRepairRequest
            {
                Date = viewModel.Date,
                TypeOfRepair = viewModel.TypeOfRepair,
                Address = viewModel.Address,
                RepairStatus = EnRepairStatus.Pending,
                Cost = 0,
                UserId = ActiveUser.User.Id
            };

            var response = await client.PostAsJsonAsync(uri, propertyRepair);

            if (response.IsSuccessStatusCode)
            {
                var apiResult = await response.Content.ReadFromJsonAsync<Result<PropertyRepairResponseDTO>>();

                if (apiResult?.Status == 0)
                {
                    TempData["successMessage"] = "Item Created.";
                    return RedirectToAction("GetRepairsByUserId", new { id = ActiveUser.User.Id });
                }
                else
                {
                    ModelState.AddModelError("", apiResult?.Message ?? "An unknown error occurred.");
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"API Error: {errorContent}");
            }
        }
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var uri = new Uri($"{client.BaseAddress}/Repair/{id}");
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var item = JsonConvert.DeserializeObject<Result<PropertyRepairResponseDTO>>(data);

                if (item == null)
                {
                    TempData["errorMessage"] = "No repairs found for this ID.";
                    return RedirectToAction("GetByID");
                }

                var viewModel = new PropertyRepairViewModel
                {
                    Id = item.Value!.Id,
                    Date = item.Value.Date,
                    Address = item.Value.Address,
                    TypeOfRepair = item.Value.TypeOfRepair,
                    Cost = item.Value.Cost,
                    RepairStatus = item.Value.RepairStatus,
                    IsActive = item.Value.IsActive,
                };

                return View(viewModel);
            }
            return View(new PropertyRepairViewModel
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
    public async Task<IActionResult> Edit(PropertyRepairViewModel model)
    {
        if (ModelState.IsValid)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var uri = new Uri($"{client.BaseAddress}/Repair/{model.Id}");
            string data = JsonConvert.SerializeObject(model);

            //TODO CREATE REQUEST AND SEND 

            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await client.PatchAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {
              
                if(ActiveUser.UserRole is EnRoleType.User)
                    return RedirectToAction("GetAll");
               return RedirectToAction("Repairs","Admin");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"API Error: {errorContent}");
            }
        }
        return View(model);
    }

    [HttpPost("deactivate/{repairId}")]
    public async Task<IActionResult> SoftDelete(int repairId)
    {
        var client = _httpClientFactory.CreateClient("ApiClient");

        var uri = new Uri($"{client.BaseAddress}/Repair/deactivate/{repairId}");

        var response = await client.PutAsync(uri, null);

        if (response.IsSuccessStatusCode)
        {
            TempData["successMessage"] = "Repair deleted successfully.";
        }
        else
        {
            TempData["errorMessage"] = "Error deleting repair.";
        }
        return RedirectToAction("GetRepairsByUserId", new { id = ActiveUser.User!.Id });
    }

   [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        if(ActiveUser.UserRole is not EnRoleType.Admin)
        {
            return Problem();
        }

        var client = _httpClientFactory.CreateClient("ApiClient");
        var apiUrl = new Uri($"{client.BaseAddress}/Repair/deactivate/{id}");

        var response = await client.DeleteAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            return Ok(); 
        }
        else
        {
            return StatusCode((int)response.StatusCode, "Failed to delete the repair.");
        }
    }

}
