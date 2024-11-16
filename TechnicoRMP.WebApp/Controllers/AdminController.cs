using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using TechnicoRMP.Shared.Dtos;
using TechnicoRMP.WebApp.Models;

namespace TechnicoRMP.WebApp.Controllers
{
    public class AdminController(IHttpClientFactory httpClientFactory) : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        [HttpGet]
        public async Task<IActionResult> UsersManagment()
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var uri = new Uri($"{client.BaseAddress}/User");

            var response = await client.GetAsync(uri);

            var res = await response.Content.ReadFromJsonAsync<List<UserDto>>();

            if(res == null)
            {
                return View(new List<UserViewmodel>());
            }

            var vm = res.Select(user => new UserViewmodel
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                VatNumber = user.VatNumber,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                IsActive = user.IsActive
            }).ToList();

            return View(vm);
        }
    }
}
