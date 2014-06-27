using NLog;
using System;

namespace NetDevPL.Logging
{
    public class NLogLogger : ILogger
    {
        private Logger _logger;

        public NLogLogger()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public NLogLogger(string loggerName)
        {
            _logger = LogManager.GetLogger(loggerName);
        }

        ~NLogLogger()
        {
            Dispose(false);
        }

        public void Debug(string message, params object[] args)
        {
            _logger.Debug(message, args);
        }

        public void DebugException(string message, Exception exception)
        {
            _logger.Debug(message, exception);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Error(string message, params object[] args)
        {
            _logger.Error(message, args);
        }

        public void ErrorException(string message, Exception exception)
        {
            _logger.Error(message, exception);
        }

        public void Fatal(string message, params object[] args)
        {
            _logger.Fatal(message, args);
        }

        public void FatalException(string message, Exception exception)
        {
            _logger.Fatal(message, exception);
        }

        public void Info(string message, params object[] args)
        {
            _logger.Info(message, args);
        }

        public void InfoException(string message, Exception exception)
        {
            _logger.Info(message, exception);
        }

        public void Trace(string message, params object[] args)
        {
            _logger.Trace(message, args);
        }

        public void TraceException(string message, Exception exception)
        {
            _logger.Trace(message, exception);
        }

        public void Warning(string message, params object[] args)
        {
            _logger.Warn(message, args);
        }

        public void WarningException(string message, Exception exception)
        {
            _logger.Warn(message, exception);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _logger = null;
            }
        }
    }
}