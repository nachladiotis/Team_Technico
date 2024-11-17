using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Technico.Api.Services;
using TechnicoRMP.Shared.Dtos;

namespace Technico.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LogsController(ILogsService logsService) : ControllerBase
{
    private readonly ILogsService _logsService = logsService;

    [HttpGet]
    public async Task<ActionResult<List<LogEntryDto>>> GetFilteredLogs(string? logLevel, string? exceptionName)
    {
        var logsResult = await _logsService.GetByFilters(logLevel,exceptionName);
        return Ok(logsResult);
    }
}
