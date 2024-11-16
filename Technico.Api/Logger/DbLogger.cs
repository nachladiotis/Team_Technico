using TechnicoRMP.Database.DataAccess;
using TechnicoRMP.Models.Logs;

namespace Technico.Api.Logger;

public class DbLogger<T>(DataStore dbContext) : ILogger where T : class
{
    private readonly DataStore _dbContext = dbContext;
    private readonly string _categoryName = typeof(T).Name ?? string.Empty;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default;

    public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Information;


    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;

        var logEntry = new LogEntry
        {
            LogDate = DateTime.UtcNow,
            LogLevel = logLevel.ToString(),
            Message = formatter(state, exception),
            StackTrace = exception?.StackTrace,
            ServiceName = _categoryName,
            ExceptionName = exception?.GetType().Name
        };

        Task.Run(async () =>
        {
            _dbContext.LogEntries.Add(logEntry);
            await _dbContext.SaveChangesAsync();
        });
    }
}
