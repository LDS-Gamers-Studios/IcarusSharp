namespace Icarus
{
    public class LogMessageDiverter<T> : ILoggerProvider
    {
        ILogger<T> Logger;

        public LogMessageDiverter(ILogger<T> logger)
        {
            Logger = logger;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return Logger;
        }

        public void Dispose() { }
    }
}
