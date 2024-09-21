﻿using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using ColorDesktop.Launcher.Helper;
using ColorDesktop.Launcher.Utils;
using Tmds.DBus.Protocol;

namespace ColorDesktop.Launcher;

public class Program
{
    public const string Version = "1.0.0";

    private static FileStream s_lock;

    public static string RunDir { get; private set; }

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 |
            SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

        TaskScheduler.UnobservedTaskException += (object? sender, UnobservedTaskExceptionEventArgs e) =>
        {
            if (e.Exception.InnerException is DBusException)
            {
                Logs.Error(App.Lang("App.Error1"), e.Exception);
                return;
            }
            PathHelper.OpenFileWithExplorer(Logs.Crash(App.Lang("Gui Error"), e.Exception));
        };

        SystemInfo.Init();

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
            PathHelper.OpenFileWithExplorer(Logs.Crash("Gui Crash", e));
            App.Exit();
        }

        s_lock.Close();
        s_lock.Dispose();
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace();

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
