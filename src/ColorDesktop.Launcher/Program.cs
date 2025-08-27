using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media;
using ColorDesktop.Api;
using ColorDesktop.Launcher.Objs;
using ColorDesktop.Launcher.Utils;
using Tmds.DBus.Protocol;

namespace ColorDesktop.Launcher;

public class Program
{
    public const string Version = "A5.20250827";
    public const string ApiVersion = LauncherApi.ApiVersion;

    private static FileStream s_lock;

    public static RunType RunType { get; private set; } = RunType.AppBuilder;

    public static string RunDir { get; private set; }

    public const string Font = "resm:ColorDesktop.Launcher.Resource.MiSans-Regular.ttf?assembly=ColorDesktop.Launcher#MiSans";

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 |
            SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

        AppDomain.CurrentDomain.UnhandledException += (a, e) =>
        {
            Logs.Error("Gui Error", e.ExceptionObject as Exception);
        };

        TaskScheduler.UnobservedTaskException += (object? sender, UnobservedTaskExceptionEventArgs e) =>
        {
            if (e.Exception.InnerException is DBusException)
            {
                Logs.Error(LangApi.GetLang("App.Error1"), e.Exception);
                return;
            }
            PathHelper.OpenFileWithExplorer(Logs.Crash(LangApi.GetLang("Gui Error"), e.Exception));
        };

        SystemInfo.Init();

        RunType = RunType.Program;

#if !DEBUG
        if (SystemInfo.Os == OsType.Linux)
        {
            RunDir = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.ColorDesktop/";
        }
        else if (SystemInfo.Os == OsType.MacOS)
        {
            RunDir = "/Users/shared/ColorDesktop/";
        }
        else
        {
            RunDir = AppContext.BaseDirectory;
        }

        try
        {
            if (!Directory.Exists(RunDir))
            {
                Directory.CreateDirectory(RunDir);
            }
            File.Create(RunDir + "temp").Close();
        }
        catch
        {
            //没有权限写文件
            RunDir = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/ColorMC/";
            RunDir = Path.GetFullPath(RunDir);
        }
        Console.WriteLine($"RunDir: {RunDir}");
#else
        RunDir = AppContext.BaseDirectory;
#endif

        try
        {
            if (IsLock(out var port))
            {
                LaunchSocketUtils.SendMessage(port).Wait();
                return;
            }

            BuildAvaloniaApp()
                 .StartWithClassicDesktopLifetime(args);
        }
        catch (Exception e)
        {
            PathHelper.OpenFileWithExplorer(Logs.Crash(AppContext.BaseDirectory + "Gui Crash", e));
            App.Exit();
        }

        s_lock?.Close();
        s_lock?.Dispose();
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        if (RunType == RunType.AppBuilder)
        {
            RunDir = AppContext.BaseDirectory;

            SystemInfo.Init();
        }
        var builder = AppBuilder.Configure<App>();
        if (SystemInfo.Os == OsType.MacOS)
        {
            var opt = new MacOSPlatformOptions()
            {
                DisableDefaultApplicationMenuItems = true,
            };
            builder.With(opt);
        }
        return builder
#if DEBUG
            .LogToTrace(Avalonia.Logging.LogEventLevel.Information)
#else
            .LogToTrace()
#endif
            .With(new FontManagerOptions
            {
                DefaultFamilyName = Font,
            })
            .UsePlatformDetect();
    }

    private static bool IsLock(out int port)
    {
        var name = RunDir + "lock";
        port = -1;
        if (File.Exists(name))
        {
            try
            {
                using var temp = File.Open(name, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch
            {
                using var temp = File.Open(name, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                byte[] temp1 = new byte[4];
                temp.ReadExactly(temp1);
                port = BitConverter.ToInt32(temp1);
                return true;
            }
        }

        return false;
    }

    public static void StartLock()
    {
        LaunchSocketUtils.Init().Wait();
        string name = RunDir + "lock";
        s_lock = File.Open(name, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        var data = BitConverter.GetBytes(LaunchSocketUtils.Port);
        s_lock.Write(data);
        s_lock.Flush();
    }

}
