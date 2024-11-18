using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TechnicoRMP.Models;
using TechnicoRMP.Shared.Dtos;
using TechnicoRMP.WebApp.Models;

namespace TechnicoRMP.WebApp.Controllers;

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

        if (res == null)
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

    [HttpPost]
    public IActionResult FilterUsers([FromBody] FilterUsersRequest request)
    {
        var filteredUsers = string.IsNullOrEmpty(request.VatNumber)
            ? request.AllUsers
            : request.AllUsers.Where(u => u.VatNumber!.Contains(request.VatNumber)).ToList();

        return PartialView("_UsersTablePartial", filteredUsers);
    }

    [HttpGet]
    public async Task<IActionResult> Logs(string? logLevel, string? exceptionName)
    {

        var client = _httpClientFactory.CreateClient("ApiClient");
        var uri = new Uri($"{client.BaseAddress}/Logs");
        var requestUri = $"{uri}?logLevel={logLevel}&exceptionName={exceptionName}";

        var response = await client.GetAsync(requestUri);

        if (!response.IsSuccessStatusCode)
        {
            return View(new List<LogEntryDto>());
        }

        var logs = await response.Content.ReadFromJsonAsync<List<LogEntryDto>>();

        var logEntries = logs!.Select(log => new LogEntryViewModel
        {
            Id = log.Id,
            LogDate = log.LogDate,
            LogLevel = log.LogLevel,
            Message = log.Message,
            StackTrace = log.StackTrace,
            ServiceName = log.ServiceName,
            ExceptionName = log.ExceptionName
        }).ToList();

        return View(logEntries);
    }

    [HttpGet("Repairs")]
    public async Task<IActionResult> Repairs(DateTime? startDate, DateTime? endDate)
    {
        var client = _httpClientFactory.CreateClient("ApiClient");
        var uri = new Uri($"{client.BaseAddress}/Repair?startDate={startDate?.ToString("yyyy-MM-dd")}&endDate={endDate?.ToString("yyyy-MM-dd")}");

        var response = await client.GetAsync(uri);

        var res = await response.Content.ReadFromJsonAsync<List<PropertyRepairResponseDTO>>();

        if (res == null)
        {
            return View(new List<PropertyRepairViewModel>());
        }

        var listOfVms = res.Select(repair => new PropertyRepairViewModel
        {
            Address = repair.Address,
            Cost = repair.Cost,
            Date = repair.Date,
            Id = repair.Id,
            IsActive = repair.IsActive,
            RepairStatus = repair.RepairStatus,
            TypeOfRepair = repair.TypeOfRepair,
            UserId = repair.UserId
        }).ToList();

        return View(listOfVms);
    }

    [HttpGet]
    public async Task<IActionResult> CreateRepair()
    {
        var client = _httpClientFactory.CreateClient("ApiClient");
        var uri = new Uri($"{client.BaseAddress}/User");
        var response = await client.GetAsync(uri);

        if (!response.IsSuccessStatusCode)
        {
            return View("Error");
        }

        var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();
        var userViewModels = users?.Select(user => new UserViewmodel
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname
        }).ToList();

        ViewData["Users"] = userViewModels;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateRepair([FromForm] CreatePropertyRepairRequest repairRequest)
    {
        var client = _httpClientFactory.CreateClient("ApiClient");
        var uri = new Uri($"{client.BaseAddress}/User");
        var response = await client.GetAsync(uri);

        if (!response.IsSuccessStatusCode)
        {
            ViewData["Users"] = new List<UserViewmodel>();
            return View(repairRequest);
        }

        var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();
        var userViewModels = users?.Select(user => new UserViewmodel
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname
        }).ToList();

        ViewData["Users"] = userViewModels;

        if (!ModelState.IsValid)
        {
            return View(repairRequest);
        }

        try
        {
            var apiResponse = await client.PostAsJsonAsync($"{client.BaseAddress}/Repair", repairRequest);

            if (apiResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Repairs");
            }

            var errorResponse = await apiResponse.Content.ReadAsStringAsync();
            var jsonObject = JsonDocument.Parse(errorResponse);
            var errorMessage = jsonObject.RootElement.GetProperty("message").GetString();
            ViewData["ErrorMessage"] = errorMessage;
            return View(repairRequest);
        }
        catch (Exception ex)
        {
            ViewData["ErrorMessage"] = $"Internal server error: {ex.Message}";
            return View(repairRequest);
        }
    }

    [HttpGet]
    public async Task<IActionResult> PropertyItems()
    {
        var client = _httpClientFactory.CreateClient("ApiClient");
        var uri = new Uri($"{client.BaseAddress}/PropertyItems");

        var response = await client.GetAsync(uri);

        var res = await response.Content.ReadFromJsonAsync<List<PropertyItemsDto>>();

        if (res == null)
        {
            return View(new List<PropertyItemsDto>());
        }

        var listOfVms = res.Select(propertyItem => new PropertyItemsDto
        {
            Address = propertyItem.Address,
            E9Number = propertyItem.E9Number,
            EnPropertyType = propertyItem.EnPropertyType,
            Id = propertyItem.Id,
            IsActive = propertyItem.IsActive,
            YearOfConstruction = propertyItem.YearOfConstruction,
        }).ToList();

        return View(listOfVms);
    }

    [HttpDelete("DeleteRepair/{id}")]
    public async Task<IActionResult> DeleteRepair(long id)
    {
        if (ActiveUser.UserRole is not EnRoleType.Admin)
        {
            return Problem();
        }

        var client = _httpClientFactory.CreateClient("ApiClient");
        var apiUrl = new Uri($"{client.BaseAddress}/Repair/{id}");

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

    [HttpGet("PropertyItems")]
    public async Task<IActionResult> PropertyItems(string? searchE9Number)
    {

        var client = _httpClientFactory.CreateClient("ApiClient");

        string url;
        if (string.IsNullOrEmpty(searchE9Number))
            url = $"{client.BaseAddress}/PropertyItem/GetAll";
        else
            url = $"{client.BaseAddress}/PropertyItem/GetBy/{searchE9Number}";

        var response = await client.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            return View(new List<PropertyItemViewModel>());

        var items = await response.Content.ReadFromJsonAsync<IEnumerable<PropertyItemsDto>>();

        if (items is null)
        {
            return View(new List<PropertyItemViewModel>());
        }

        var listOfVms = items.Select(propertyItem => new PropertyItemViewModel
        {
            Address = propertyItem.Address!,
            E9Number = propertyItem.E9Number!,
            EnPropertyType = propertyItem.EnPropertyType,
            Id = propertyItem.Id,
            IsActive = propertyItem.IsActive,
            YearOfConstruction = propertyItem.YearOfConstruction,
        }).ToList();



        return View(listOfVms);
    }

    [HttpDelete("/DeletePropertyItem/{id}")]
    public async Task<IActionResult> DeletePropertyItem(long id)
    {
        if (ActiveUser.UserRole is not EnRoleType.Admin)
            return BadRequest();

        var client = _httpClientFactory.CreateClient("ApiClient");
        var response = await client.DeleteAsync($"{client.BaseAddress}/PropertyItem/Delete/{id}");

        if (!response.IsSuccessStatusCode)
            return BadRequest("Failed to delete the item.");
        return Ok();
    }
}



