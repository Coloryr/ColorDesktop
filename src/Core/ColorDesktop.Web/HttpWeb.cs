using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using ColorDesktop.Api;
using Microsoft.Extensions.FileProviders;

namespace ColorDesktop.Web;

public static class HttpWeb
{
    public static int Port { get; private set; }

    public static void Start()
    {
        var builder = WebApplication.CreateBuilder();

        builder.Logging.ClearProviders();
        builder.Logging.AddProvider(new LoggerProvider());

        // 自动选择一个可用端口
        Port = GetAvailablePort();
        builder.WebHost.UseKestrel(options =>
        {
            options.Limits.MaxRequestBodySize = null;
        });
        builder.WebHost.UseUrls($"http://localhost:{Port}");

        var app = builder.Build();

        //if (env.IsDevelopment())
        //{
        //    app.UseDeveloperExceptionPage();
        //}

        // 配置SPA静态文件
        app.UseDefaultFiles();
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(PluginManager.RunDir)
        });

        app.Start();

        Console.WriteLine("http start in " + Port);
    }

    private static int GetAvailablePort()
    {
        // 使用 TcpListener 来动态选择一个可用的端口
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        int port = ((IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();
        return port;
    }
}

public class LoggerProvider : ILoggerProvider
{
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