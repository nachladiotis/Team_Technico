using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TechnicoMVCApp.Models;

namespace TechnicoMVCApp.Controllers
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

        [HttpGet]
    
        public IActionResult Index()
        {

            List<PropertyItemViewModel> ItemList = new List<PropertyItemViewModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/propertyItem").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                ItemList = JsonConvert.DeserializeObject<List<PropertyItemViewModel>>(data);
            }
            return View(ItemList);
        }
    }
}
