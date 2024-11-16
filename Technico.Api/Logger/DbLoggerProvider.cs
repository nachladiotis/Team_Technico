using TechnicoRMP.Database.DataAccess;

namespace Technico.Api.Logger
{
    public class DbLoggerProvider(DataStore dbContext) : ILoggerProvider
    {
        private readonly DataStore _dbContext = dbContext;

        public ILogger CreateLogger(string categoryName)
        {
            var loggerType = Type.GetType(categoryName) ?? typeof(object);
            return (ILogger)Activator.CreateInstance(typeof(DbLogger<>).MakeGenericType(loggerType), _dbContext)!;
        }

        public void Dispose()
        {
            
        }
    }
}
