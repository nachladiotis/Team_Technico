﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using TechnicoRMP.Models;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;
using TechnicoRMP.WebApp.Models;

namespace TechnicoRMP.WebApp.Controllers;

public class PropertyItemController(IHttpClientFactory httpClientFactory) : Controller
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task<IActionResult> Index(string searchString)
    {
        var client = _httpClientFactory.CreateClient("ApiClient");
        var uri = new Uri($"{client.BaseAddress}/propertyItem/GetPropertyItems");
        List<PropertyItemViewModel> ItemList = new List<PropertyItemViewModel>();
        HttpResponseMessage response = await client.GetAsync(uri);

        if (response.IsSuccessStatusCode)
        {
            string data = response.Content.ReadAsStringAsync().Result;
            ItemList = JsonConvert.DeserializeObject<List<PropertyItemViewModel>>(data)!;
            // Filter down if necessary
            if (!String.IsNullOrEmpty(searchString))
            {
                ItemList = ItemList.Where(p => p.E9Number == searchString).ToList();
            }
            // Pass your list out to your view
            return View(ItemList.ToList());
        }
        return View(ItemList);
    }


    [HttpGet("PropertyItem/GetPropertyItemByUserId/{id}")]
    public async Task<IActionResult> GetPropertyItemByUserId(int id)
    {
        //var UserId = ActiveUser.User!.Id;
        var client = _httpClientFactory.CreateClient("ApiClient");
        List<PropertyItemViewModel> ItemList = new List<PropertyItemViewModel>();
        HttpResponseMessage response = await client.GetAsync(client.BaseAddress + "/PropertyItem/GetPropertyItemByUserId/" + id);

        if (response.IsSuccessStatusCode)
        {
            string data = await response.Content.ReadAsStringAsync();
            var ownerItemList = JsonConvert.DeserializeObject<Result<PropertyItemsByUserDto>>(data);
            if (ownerItemList.Status == -1)
            {
                return View(ItemList);
            }
            var userId = ownerItemList.Value.UserDto.Id;
            foreach (var item in ownerItemList.Value.PropertyItems)
            {
                var viewmodel = new PropertyItemViewModel
                {
                    Id = item.Id,
                    Address = item.Address,
                    E9Number = item.E9Number,
                    EnPropertyType = item.EnPropertyType,
                    IsActive = item.IsActive,
                    YearOfConstruction = item.YearOfConstruction,
                    UserId = userId

                };
                ItemList.Add(viewmodel);
            }
            return View(ItemList);
        }
        return View(ItemList);
    }



    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePropertyItemViewmodel model)
    {
        try
        {
            string data = JsonConvert.SerializeObject(model);
            var client = _httpClientFactory.CreateClient("ApiClient");
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(client.BaseAddress + "/propertyItem/Create", content);
            if (response.IsSuccessStatusCode)
            {
                TempData["successMessage"] = "Item Created.";
                if(ActiveUser.UserRole is EnRoleType.User)
                    return RedirectToAction("Index");
                return RedirectToAction("UsersManagment","Admin");
            }
        }
        catch (Exception ex)
        {
            throw;
        }
        return View();
    }


    [HttpGet]
    public IActionResult CreateByUserId()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateByUserId(PropertyItemViewModel model)
    {
        var request = new CreatePropertyItemRequest
        {
            Address = model.Address,
            E9Number = model.E9Number,
            EnPropertyType = model.EnPropertyType,
            IsActive = model.IsActive,
            UserId = model.UserId,
            YearOfConstruction = model.YearOfConstruction
        };
        var client = _httpClientFactory.CreateClient("ApiClient");
        var uri = new Uri(client.BaseAddress + "/PropertyItem/Create");
        HttpResponseMessage response = await client.PostAsJsonAsync(uri, request);
        if (response.IsSuccessStatusCode)
        {
            TempData["successMessage"] = "Item Created.";
            return RedirectToAction("GetPropertyItemByUserId", new { id = model.UserId });
        }
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress + "/PropertyItem/GetPropertyItemById/" + id);
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var item = JsonConvert.DeserializeObject<Result<CreatePropertyItemResponse>>(data);
                var viewmodel = new PropertyItemViewModel
                {
                    Id = item.Value.Id,
                    Address = item.Value.Address,
                    E9Number = item.Value.E9Number,
                    EnPropertyType = item.Value.EnPropertyType,
                    IsActive = item.Value.IsActive,
                    YearOfConstruction = item.Value.YearOfConstruction
                };
                return View(viewmodel);

            }
            return View(new PropertyItemViewModel
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
    public async Task<IActionResult> Edit(PropertyItemViewModel model)
    {
        var client = _httpClientFactory.CreateClient("ApiClient");
        string data = JsonConvert.SerializeObject(model);
        StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PutAsync(client.BaseAddress + "/propertyItem/Update", content);
        if (response.IsSuccessStatusCode)
        {
            if(ActiveUser.UserRole == EnRoleType.Admin)
            {
                return RedirectToAction("PropertyItems", "Admin");
            }

            return RedirectToAction("GetPropertyItemByUserId", new { id = @ActiveUser.User!.Id });
        }
        return View();
    }


    [HttpGet]
    public async Task<IActionResult> DeleteByUser(int id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress + "/propertyItem/GetPropertyItemById/" + id);
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var item = JsonConvert.DeserializeObject<Result<CreatePropertyItemResponse>>(data);
                var viewmodel = new PropertyItemViewModel
                {
                    Id = item.Value.Id,
                    Address = item.Value.Address,
                    E9Number = item.Value.E9Number,
                    EnPropertyType = item.Value.EnPropertyType,
                    IsActive = item.Value.IsActive,
                    YearOfConstruction = item.Value.YearOfConstruction
                };
                return View(viewmodel);
            }
            return View(new PropertyItemViewModel
            {
                Id = id
            });
        }
        catch (Exception ex)
        {
            return View();
        }
    }

    [HttpPost, ActionName("DeleteByUser")] //PropertyItem/Delete/19
    public IActionResult DeleteConfirmed(int id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            HttpResponseMessage response = client.DeleteAsync(client.BaseAddress + "/propertyItem/Delete/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("GetPropertyItemByUserId", new { id = @ActiveUser.User!.Id });
            }
        }
        catch (Exception ex)
        {
            return View();
        }
        return View();
    }

}
