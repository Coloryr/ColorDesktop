using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;

namespace ColorDesktop.Web;

public static class HttpWeb
{
    private static readonly ConcurrentDictionary<string, StaticFileOptions> s_dynamicFileOptions = [];
    private static readonly FileExtensionContentTypeProvider s_typeProvider = new();
    private static readonly ConcurrentDictionary<string, string> s_tokens=[];

    private static WebApplication s_app;

    public static int Port { get; private set; }

    public static void Start()
    {
        var builder = WebApplication.CreateBuilder();
        var log = new LoggerProvider();
        builder.Logging.ClearProviders();
        builder.Logging.AddProvider(log);

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

        //app.UseDefaultFiles();
        //app.UseStaticFiles(new StaticFileOptions
        //{
        //    FileProvider = new PhysicalFileProvider(PluginManager.RunDir)
        //});

        // Middleware to handle dynamic static files
        app.Use(async (context, next) =>
        {
            string path = context.Request.Path.Value!.Trim('/');
            string file;
            if (context.Request.Cookies.TryGetValue("colordesktop", out var uuid)
            && s_tokens.TryGetValue(uuid, out var key))
            {
                file = path;
            }
            else
            {
                key = path.Split('/')[0];
                file = path[key.Length..];
                if (string.IsNullOrWhiteSpace(file))
                {
                    file = "index.html";
                }
            }

            if (s_dynamicFileOptions.TryGetValue(key, out var options))
            {
                var fileProvider = options.FileProvider!;
                
                var fileInfo = fileProvider.GetFileInfo(file);

                if (uuid == null)
                {
                    do
                    {
                        uuid = GenUUID();
                    }
                    while (s_tokens.ContainsKey(uuid));
                }
                s_tokens.TryAdd(uuid, key);

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

    private static string GenUUID()
    {
        return Guid.NewGuid().ToString().ToLower().Replace("-", "");
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
        // 使用 TcpListener 来动态选择一个可用的端口
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
