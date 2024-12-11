using System.Collections.Concurrent;
using ColorDesktop.Api;

namespace ColorDesktop.Web;

public class LoggerProvider : ILoggerProvider
{
    public class Logger : ILogger, IDisposable
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return this;
        }

        public void Dispose()
        {

        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
                            Exception? exception, Func<TState, Exception, string> formatter)
        {
            //if (eventId.Id == 100 || eventId.Id == 101)
            //    return;
            if (logLevel is LogLevel.Warning)
            {
                Logs.Warn($"{logLevel}-{eventId.Id} {state} {exception} {exception?.StackTrace}");
            }
            else if (logLevel is LogLevel.Error)
            {
                Logs.Error($"{logLevel}-{eventId.Id} {state} {exception} {exception?.StackTrace}");
            }
            else
            {
                Logs.Info($"{logLevel}-{eventId.Id} {state} {exception}");
            }
        }
    }

    private readonly ConcurrentDictionary<string, Logger> _loggers = new();

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, name => new Logger());
    }

    public void Dispose()
    {
        _loggers.Clear();
    }
}
