namespace TechnicoRMP.WebApp.Models;

public class LogEntryViewModel
{
    public long Id { get; set; }
    public DateTime LogDate { get; set; }
    public string? LogLevel { get; set; }
    public string? Message { get; set; }
    public string? StackTrace { get; set; }
    public string? ServiceName { get; set; }
    public string? ExceptionName { get; set; }
}
