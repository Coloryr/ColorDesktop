﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using ColorDesktop.Launcher.Utils;

namespace ColorDesktop.Launcher.Manager;

public static class TempManager
{
    private static string s_run;

    public static void Init()
    {
        s_run = Program.RunDir + "tmp";
        Directory.CreateDirectory(s_run);
    }

    public static async Task<Bitmap?> LoadImage(string url)
    {
        var file = s_run + "/" + GenSha1(url);

        try
        {
            if (File.Exists(file))
            {
                return new Bitmap(file);
            }
            using var stream = await LauncherUtils.Client.GetStreamAsync(url);
            using var temp = File.Open(file, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
            stream.CopyTo(temp);
            temp.Seek(0, SeekOrigin.Begin);
            return new Bitmap(temp);
        }
        catch
        {

        }
        return null;
    }

    public static async Task<bool> Download(string url, string local, string sha1)
    {
        try
        {
            var info = new FileInfo(local);
            info.Directory?.Create();
            using var stream = await LauncherUtils.Client.GetStreamAsync(url);
            using var temp = File.Open(local, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
            stream.CopyTo(temp);
            temp.Seek(0, SeekOrigin.Begin);
            var sha2 = GenSha1(temp);
            if (sha1 != sha2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        catch
        {

        }

        return false;
    }

    /// <summary>
    /// 获取SHA1值
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public static string GenSha1(Stream stream)
    {
        var text = new StringBuilder();
        foreach (byte item in SHA1.HashData(stream))
        {
            text.AppendFormat("{0:x2}", item);
        }
        return text.ToString().ToLower();
    }

    /// <summary>
    /// 获取SHA1值
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public static string GenSha1(string data)
    {
        var text = new StringBuilder();
        foreach (byte item in SHA1.HashData(Encoding.UTF8.GetBytes(data)))
        {
            text.AppendFormat("{0:x2}", item);
        }
        return text.ToString().ToLower();
    }

    private static string GenUUID()
    {
        return Guid.NewGuid().ToString().ToLower();
    }
}
