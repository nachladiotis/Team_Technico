using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using TechnicoRMP.Models;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;
using TechnicoRMP.WebApp.Models;

namespace TechnicoRMP.WebApp.Controllers
{
    public class PropertyItemController : Controller
    {
        Uri baseAdsress = new Uri("https://localhost:44322/api");
        private readonly HttpClient _client;

        public PropertyItemController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAdsress;
        }

        public async Task<IActionResult> Index(string searchString)
        {

            List<PropertyItemViewModel> ItemList = new List<PropertyItemViewModel>();
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "/propertyItem/GetPropertyItems");

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                ItemList = JsonConvert.DeserializeObject<List<PropertyItemViewModel>>(data);
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


        [HttpGet("PropertyItem/GetPropertyItemByUserId/{UserId}")]
        public async Task<IActionResult> GetPropertyItemByUserId(int UserId) //PropertyItem/GetPropertyItemByUserId/1
        {

            List<PropertyItemViewModel> ItemList = new List<PropertyItemViewModel>();
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "/propertyItem/GetPropertyItemByUserId/" + UserId);

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var ownerItemList = JsonConvert.DeserializeObject<Result<PropertyItemsByUserDto>>(data);
                if(ownerItemList.Status == -1)
                {
                    return View(ItemList);
                }
                var userId = ownerItemList.Value.USerDto.Id;
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

        [HttpGet]
        public async Task<IActionResult> CreateNew(CreatePropertyItemViewmodel model)
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
            var uri = new Uri(_client.BaseAddress + "/propertyItem/CreatePropertyItemByUserId/");
            HttpResponseMessage response = await _client.PostAsJsonAsync(uri, request);
            if (response.IsSuccessStatusCode)
            {
                TempData["successMessage"] = "Item Created.";
                return RedirectToAction("CreateByUserId");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "/PropertyItem/GetPropertyItemById/" + id);
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
