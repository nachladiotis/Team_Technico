using Microsoft.EntityFrameworkCore;
using TechnicoRMP.Database.DataAccess;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;

namespace Technico.Api.Services;

public sealed class LogsService(DataStore dbContext) : ILogsService
{
    private readonly DataStore _dbContext = dbContext;

    public async Task<IEnumerable<LogEntryDto>> GetAll()
    {
        var logsDtos = await _dbContext.LogEntries
             .Select(log => new LogEntryDto
             {
                 Id = log.Id,
                 ExceptionName = log.ExceptionName,
                 LogDate = log.LogDate,
                 LogLevel = log.LogLevel,
                 Message = log.Message,
                 ServiceName = log.ServiceName,
                 StackTrace = log.StackTrace,
             })
             .ToListAsync();

        return logsDtos;
    }

    public async Task<IEnumerable<LogEntryDto>> GetByFilters(string? logLevel,string? exceptionName)
    {
        var query = _dbContext.LogEntries.AsQueryable();

     
        if (!string.IsNullOrEmpty(logLevel))
        {
            query = query.Where(log => log.LogLevel!.Contains(logLevel));
        }

        if (!string.IsNullOrEmpty(exceptionName))
        {
            query = query.Where(log => log.ExceptionName!.Contains(exceptionName));
        }

        var logsDtos = await query
          .Select(log => new LogEntryDto
          {
              Id = log.Id,
              ExceptionName = log.ExceptionName,
              LogDate = log.LogDate,
              LogLevel = log.LogLevel,
              Message = log.Message,
              ServiceName = log.ServiceName,
              StackTrace = log.StackTrace,
          })
          .AsNoTracking()
          .ToListAsync();

        return logsDtos;
    }
}
