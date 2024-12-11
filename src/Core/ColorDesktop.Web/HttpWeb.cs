using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ColorDesktop.Web;

public static class HttpWeb
{
    private static readonly ConcurrentDictionary<string, StaticFileOptions> s_dynamicFileOptions = [];
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

        //if (env.IsDevelopment())
        //{
        //    app.UseDeveloperExceptionPage();
        //}

        app.Use(async (context, next) =>
        {
            string path = context.Request.Path.Value!.Trim('/');
            string file;
            if (context.Request.Cookies.TryGetValue("colordesktop", out var uuid))
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

            if (context.Request.Method == "POST")
            {
                if (uuid == null)
                {
                    context.Response.StatusCode = 301;
                    await context.Response.WriteAsJsonAsync(new { msg = "uuid error" });
                    return;
                }
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
            else if(InstanceManager.Instances.TryGetValue(uuid, out var key))
            {
                if (s_dynamicFileOptions.TryGetValue(key.Plugin, out var options))
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
        s_dynamicFileOptions.TryAdd(id, staticFileOptions);
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
}
