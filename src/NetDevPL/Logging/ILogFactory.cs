using System;

namespace NetDevPL.Logging
{
    //cr:mmisztal1980
    public interface ILogFactory : IDisposable
    {
        ILogger CreateLogger(string loggerName);
    }
}