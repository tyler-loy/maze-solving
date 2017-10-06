namespace mazesolvinglib.Utility
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger(string location);
    }
}
