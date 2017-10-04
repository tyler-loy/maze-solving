using System;

namespace mazesolvinglib.Utility
{
    public class ConsoleLogger : ILogger
    {
        private readonly string _location;

        public ConsoleLogger(string location)
        {
            _location = location;
        }

        public void Log(string message)
        {
            Console.WriteLine($"[{DateTime.Now.TimeOfDay.ToString("g")}][{_location}] {message}");
        }
    }
}