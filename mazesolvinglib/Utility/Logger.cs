using System;

namespace mazesolvinglib.Utility
{
    public class Logger : ILogger
    {
        private readonly string _location;

        public Logger(string location)
        {
            _location = location;
        }

        public void Log(string message)
        {
            Console.WriteLine($"[{DateTime.Now.TimeOfDay.ToString("g")}][{_location}] {message}");
        }
    }
}