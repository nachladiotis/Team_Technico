using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using Newtonsoft.Json;
using Technico.Api.Services;
using TechnicoRMP.Database.DataAccess;
using TechnicoRMP.Models;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;
using TechnicoRMP.WebApp.Data;
using TechnicoRMP.WebApp.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TechnicoRMP.WebApp.Controllers
{
    public class PropertyRepairsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PropertyRepairsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: PropertyRepairs/GetAll
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: PropertyRepairs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PropertyRepairViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                var uri = new Uri($"{client.BaseAddress}/Repair/Create");

                var Userid = 1;
                var propertyRepair = new CreatePropertyRepairRequest
                {
                    Date = viewModel.Date,
                    TypeOfRepair = viewModel.TypeOfRepair,
                    Address = viewModel.Address,
                    RepairStatus = viewModel.RepairStatus,
                    Cost = viewModel.Cost,
                    UserId = Userid
                };

                var response = await client.PostAsJsonAsync(uri, propertyRepair);

                if (response.IsSuccessStatusCode)
                {
                    var apiResult = await response.Content.ReadFromJsonAsync<Result<PropertyRepairResponseDTO>>();

                    if (apiResult?.Status == 0)
                    {
                        TempData["successMessage"] = "Item Created.";
                        return RedirectToAction("GetAll");
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

                    var result = await response.Content.ReadFromJsonAsync<PropertyRepairResponseDTO>();

                    if (result == null)
                    {
                        TempData["errorMessage"] = "No repairs found for this ID.";
                        return RedirectToAction("GetByID");
                    }

                    var viewModel = new PropertyRepairViewModel
                    {
                        Id = item.Value.Id,
                        Date = item.Value.Date,
                        Address = item.Value.Address,
                        TypeOfRepair = item.Value.TypeOfRepair,
                        Cost = item.Value.Cost,
                        RepairStatus = item.Value.RepairStatus,
                        IsActive = item.Value.IsActive,
                        UserId = 1
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
                // If an error occurs, return an empty view
                return View();
            }
        }

        [HttpPatch]
        public async Task<IActionResult> Edit(PropertyRepairViewModel model)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                var uri = new Uri($"{client.BaseAddress}/Repair/{model.Id}");
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PatchAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Repair updated successfully!";
                    return RedirectToAction("GetAll");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", $"API Error: {errorContent}");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");

            var uri = new Uri($"{client.BaseAddress}/Repair/deactivate/{id}");

            var response = await client.PostAsync(uri, null);

            if (response.IsSuccessStatusCode)
            {
                TempData["successMessage"] = "Repair deactivated successfully.";
            }
            else
            {
                TempData["errorMessage"] = "Error deactivating repair.";
            }

            return RedirectToAction("GetAll");
        }

    }
}
