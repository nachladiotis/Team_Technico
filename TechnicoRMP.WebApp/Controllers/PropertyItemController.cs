using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;
using TechnicoRMP.WebApp.Models;

namespace TechnicoRMP.WebApp.Controllers
{
    public class PropertyItemController : Controller
    {
        Uri baseAdsress = new Uri("https://localhost:7038/api");
        private readonly HttpClient _client;

        public PropertyItemController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAdsress;
        }

        public async Task<IActionResult> Index()
        {
            List<PropertyItemViewModel> ItemList = new List<PropertyItemViewModel>();
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "/propertyItem/GetPropertyItems");

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                ItemList = JsonConvert.DeserializeObject<List<PropertyItemViewModel>>(data);
            }
            return View(ItemList);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PropertyItemViewModel model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync(_client.BaseAddress + "/propertyItem/Create", content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Item Created.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/PropertyItem/GetPropertyItemById/" + id).Result;
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
            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync(_client.BaseAddress + "/propertyItem/Update", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/propertyItem/GetPropertyItemById/" + id).Result;
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

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "/propertyItem/Delete/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return View();
            }
            return View();
        }



    }
}
