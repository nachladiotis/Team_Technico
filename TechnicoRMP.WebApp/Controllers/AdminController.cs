using Microsoft.AspNetCore.Mvc;
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

    [HttpGet]
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

}



