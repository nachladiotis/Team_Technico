using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
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

        public IActionResult Index()
        {
            List<PropertyItemViewModel> ItemList = new List<PropertyItemViewModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/propertyItem/GetPropertyItems").Result;

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
        public IActionResult Create(PropertyItemViewModel model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/propertyItem/Create", content).Result;
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
        public IActionResult Edit(int id)
        {
            try
            {
                PropertyItemViewModel item = new PropertyItemViewModel();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/PropertyItem/GetPropertyItemById/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    item = JsonConvert.DeserializeObject<PropertyItemViewModel>(data);
                }
                return View(item);
            }
            catch (Exception)
            {
                return View();
            }

        }

        [HttpPost]
        public IActionResult Edit(PropertyItemViewModel model)
        {
            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PutAsync(_client.BaseAddress + "/propertyItem/Update", content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                PropertyItemViewModel item = new PropertyItemViewModel();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/propertyItem/GetPropertyItemById/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    item = JsonConvert.DeserializeObject<PropertyItemViewModel>(data);
                }
                return View(item);
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
