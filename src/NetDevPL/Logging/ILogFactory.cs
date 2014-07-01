using System;

namespace NetDevPL.Logging
{
    public interface ILogFactory : IDisposable
    {
        ILogger CreateLogger(string loggerName);
    }
}