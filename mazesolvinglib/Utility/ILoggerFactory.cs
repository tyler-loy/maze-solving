using System;
using System.Collections.Generic;
using System.Text;

namespace mazesolvinglib.Utility
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger(string location);
    }
}
