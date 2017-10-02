namespace mazesolvinglib.Utility
{
    public class LoggerFactory : ILoggerFactory
    {
        public ILogger CreateLogger(string location)
        {
            return new Logger(location);
        }
    }
}