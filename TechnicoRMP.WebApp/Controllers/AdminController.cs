using Microsoft.AspNetCore.Mvc;
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
}



