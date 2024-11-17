using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;

namespace Technico.Api.Services;

public interface ILogsService
{
   Task<IEnumerable<LogEntryDto>> GetAll(); 
   Task<IEnumerable<LogEntryDto>> GetByFilters(string? logLevel, string? exceptionName); 
}
