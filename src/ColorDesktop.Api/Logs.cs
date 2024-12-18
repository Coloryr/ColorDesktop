﻿using System.Collections.Concurrent;

namespace ColorDesktop.Api;

public static class Logs
{
    private static readonly ConcurrentBag<string> s_bags = [];

    private static string s_local;
    private static StreamWriter s_writer;
    private static readonly Thread t_log = new(Run)
    {
        Name = "Log"
    };
    private static bool s_run = false;
    private static string s_version;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="dir">运行的路径</param>
    public static void Init(string dir, string version)
    {
        if (s_run)
        {
            return;
        }

        s_version = version;
        s_local = dir;
        try
        {
            var stream = File.Open(s_local + "logs.log", FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
            stream.Seek(0, SeekOrigin.End);
            s_writer = new(stream)
            {
                AutoFlush = true
            };
            s_run = true;
            t_log.Start();
        }
        catch
        {

        }
    }

    /// <summary>
    /// 运行
    /// </summary>
    private static void Run()
    {
        while (s_run)
        {
            lock (s_bags)
            {
                while (s_bags.TryTake(out var item))
                {
                    s_writer.WriteLine(item);
                }
            }

            Thread.Sleep(100);
        }
    }

    /// <summary>
    /// 停止
    /// </summary>
    public static void Stop()
    {
        s_run = false;
        lock (s_bags)
        {
            while (s_bags.TryTake(out var item))
            {
                s_writer.WriteLine(item);
            }
            s_writer.Dispose();
        }
    }

    /// <summary>
    /// 添加数据
    /// </summary>
    /// <param name="text"></param>
    private static void AddText(string text)
    {
        if (!s_run)
            return;

        s_bags.Add(text);
    }

    /// <summary>
    /// 信息
    /// </summary>
    /// <param name="data"></param>
    public static void Info(string data)
    {
        string text = $"[{DateTime.Now}][Info]{data}";
        Console.WriteLine(text);
        AddText(text);
    }

    /// <summary>
    /// 警告
    /// </summary>
    /// <param name="data"></param>
    public static void Warn(string data)
    {
        string text = $"[{DateTime.Now}][Warn]{data}";
        Console.WriteLine(text);
        AddText(text);
    }

    /// <summary>
    /// 错误
    /// </summary>
    /// <param name="data"></param>
    public static void Error(string data)
    {
        string text = $"[{DateTime.Now}][Error]{data}";
        Console.WriteLine(text);
        AddText(text);
    }

    /// <summary>
    /// 错误
    /// </summary>
    /// <param name="data"></param>
    /// <param name="e"></param>
    public static void Error(string data, Exception? e)
    {
        string text = $"[{DateTime.Now}][Error]{data}{Environment.NewLine}{e}";
        Console.WriteLine(text);
        AddText(text);
    }

    /// <summary>
    /// 保存崩溃日志
    /// </summary>
    /// <param name="data">消息</param>
    /// <param name="e">错误内容</param>
    /// <returns></returns>
    public static string Crash(string data, Exception e)
    {
        var date = DateTime.Now;
        string text = $"Version:{s_version}{Environment.NewLine}" +
            $"System:{SystemInfo.System}{Environment.NewLine}" +
            $"SystemName:{SystemInfo.SystemName}{Environment.NewLine}" +
            $"{data}{Environment.NewLine}" +
            $"{e}";

        var file = $"{s_local}{date.Year}_{date.Month}_{date.Day}_" +
            $"{date.Hour}_{date.Minute}_{date.Second}_crash.log";

        File.WriteAllText(file, text);

        return file;
    }
}
