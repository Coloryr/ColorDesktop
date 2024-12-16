using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using ColorDesktop.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ColorDesktop.Web;

internal static class HttpWeb
{
    private static readonly ConcurrentDictionary<string, StaticFileOptions> s_file = [];
    private static readonly ConcurrentDictionary<string, IHttpRoute> s_routes = [];
    private static readonly FileExtensionContentTypeProvider s_typeProvider = new();

    private static WebApplication s_app;

    public static int Port { get; private set; }

    public static void Start()
    {
        var builder = WebApplication.CreateBuilder();
        var log = new LoggerProvider();
        builder.Logging.ClearProviders();
        builder.Logging.AddProvider(log);

        Port = GetAvailablePort();
        builder.WebHost.UseKestrel(options =>
        {
            options.Limits.MaxRequestBodySize = null;
        });
        builder.WebHost.UseUrls($"http://localhost:{Port}");

        var app = builder.Build();

        app.Use(async (context, next) =>
        {
            string path = context.Request.Path.Value!.Trim('/');
            string file;
            if (context.Request.Cookies.TryGetValue("colordesktop", out var uuid) && path != uuid)
            {
                file = path;
            }
            else
            {
                uuid = path.Split('/')[0];
                file = path[uuid.Length..];
                if (string.IsNullOrWhiteSpace(file))
                {
                    file = "index.html";
                }
            }
            if (uuid == null || !InstanceManager.Instances.TryGetValue(uuid, out var key))
            {
                context.Response.StatusCode = 301;
                await context.Response.WriteAsJsonAsync(new { msg = "uuid error" });
                return;
            }

            if (s_routes.TryGetValue(key.Plugin, out var route))
            {
                context.Response.Cookies.Append("colordesktop", uuid);
                var res = await route.Process(context);
                if (res)
                {
                    return;
                }
            }
            if (context.Request.Method == "POST")
            {
                if (file == "getConfig")
                {
                    if (!context.Request.Form.TryGetValue("file", out var file1))
                    {
                        context.Response.StatusCode = 301;
                        await context.Response.WriteAsJsonAsync(new { msg = "file error" });
                    }

                    string file2 = WebDesktop.InstanceLocal + "/" + uuid + "/" + file1 + ".json";

                    context.Response.Cookies.Append("colordesktop", uuid);
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = 200;

                    if (File.Exists(file2))
                    {
                        using var stream = File.OpenRead(file2);
                        await stream.CopyToAsync(context.Response.Body);
                    }
                    else
                    {
                        await context.Response.WriteAsJsonAsync(new { });
                    }

                    return;
                }
                else if (file == "setConfig")
                {
                    if (!context.Request.Headers.TryGetValue("file", out var file1))
                    {
                        context.Response.StatusCode = 301;
                        await context.Response.WriteAsJsonAsync(new { msg = "file error" });
                        return;
                    }

                    string file2 = WebDesktop.InstanceLocal + "/" + uuid + "/" + file1 + ".json";
                    try
                    {
                        using var stream = File.Create(file2);
                        await context.Request.Body.CopyToAsync(stream);
                    }
                    catch (Exception e)
                    {
                        context.Response.StatusCode = 400;
                        await context.Response.WriteAsJsonAsync(new { msg = e.ToString() });
                        return;
                    }

                    context.Response.StatusCode = 200;
                    await context.Response.WriteAsJsonAsync(new { msg = "ok" });
                }
            }
            else if (s_file.TryGetValue(key.Plugin, out var options))
            {
                var fileProvider = options.FileProvider!;

                var fileInfo = fileProvider.GetFileInfo(file);

                if (fileInfo.Exists)
                {
                    if (!s_typeProvider.TryGetContentType(fileInfo.Name, out var contentType))
                    {
                        contentType = "application/octet-stream";
                    }

                    context.Response.Cookies.Append("colordesktop", uuid);
                    context.Response.ContentType = contentType;
                    using var stream = fileInfo.CreateReadStream();
                    await stream.CopyToAsync(context.Response.Body);
                    return;
                }
            }

            await next();
        });

        app.Start();

        s_app = app;

        Console.WriteLine("http start in " + Port);
    }

    public static void AddPlugin(string id, string dir)
    {
        var folderName = Path.GetFileName(dir);
        var fileProvider = new PhysicalFileProvider(dir);
        var staticFileOptions = new StaticFileOptions
        {
            FileProvider = fileProvider,
            RequestPath = "/" + folderName
        };
        s_file.TryAdd(id, staticFileOptions);
    }

    public static void AddRoute(IPlugin plugin, IHttpRoute route)
    {
        var id = LauncherApi.Hook.GetPluginId(plugin);
        if (id == null)
        {
            return;
        }
        s_routes[id] = route;
    }

    public static void RemoveRoute(IPlugin plugin)
    {
        var id = LauncherApi.Hook.GetPluginId(plugin);
        if (id == null)
        {
            return;
        }
        s_routes.Remove(id, out _);
    }

    private static int GetAvailablePort()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        int port = ((IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();
        return port;
    }

    public static void Stop()
    {
        s_app.StopAsync();
    }

    public static void Clear()
    {
        s_file.Clear();
    }
}
