using System.Runtime.InteropServices;
using Avalonia.Media.Imaging;
using Newtonsoft.Json;

namespace ColorDesktop.PGLauncherPlugin.ColorMC;

/// <summary>
/// 游戏实例
/// </summary>
public partial record GameSettingObj
{
    public string UUID { get; set; }
    /// <summary>
    /// 游戏名
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 路径名
    /// </summary>
    public string DirName { get; set; }
    /// <summary>
    /// 实例组名
    /// </summary>
    public string? GroupName { get; set; }
    /// <summary>
    /// 图标
    /// </summary>
    public string? Icon { get; set; }
}

public static class ColorMCUtils
{
    public static string GetRunDir()
    {
        var path = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/ColorMC/run";
        if (File.Exists(path))
        {
            var dir = File.ReadAllText(path);
            if (Directory.Exists(dir))
            {
                return dir;
            }
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.ColorMC/";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return "/Users/shared/ColorMC/";
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(PGColorMCPlugin.Config.ColorMC) && File.Exists(PGColorMCPlugin.Config.ColorMC))
            {
                var info = new FileInfo(PGColorMCPlugin.Config.ColorMC);
                return info.DirectoryName + "/" ?? "";
            }

            return "";
        }
    }

    public static List<GameSettingObj>? GetGames()
    {
        var list1 = new List<GameSettingObj>();
        var dir = GetRunDir() + "minecraft/instances";
        if (!Directory.Exists(dir))
        {
            return null;
        }

        var list = Directory.GetDirectories(dir);
        foreach (var item in list)
        {
            var item1 = LoadInstance(item);
            if (item1 != null)
            {
                list1.Add(item1);
            }
        }
        return list1;
    }

    public static GameSettingObj? LoadInstance(string item)
    {
        var file = Path.GetFullPath(item + "/game.json");
        if (File.Exists(file))
        {
            try
            {
                return JsonConvert.DeserializeObject<GameSettingObj>(File.ReadAllText(file));
            }
            catch (Exception e)
            {

            }
        }

        return null;
    }

    public static Bitmap? GetIcon(GameSettingObj setting)
    {
        if (string.IsNullOrWhiteSpace(setting.Icon))
        {
            return null;
        }
        var dir = GetRunDir() + "minecraft/instances/" + setting.DirName + "/" + setting.Icon;
        if (File.Exists(dir))
        {
            try
            {
                using var stream = File.OpenRead(dir);
                return Bitmap.DecodeToWidth(stream, 80);
            }
            catch
            {

            }
        }

        return null;
    }

    public static void Launch(GameSettingObj obj)
    {
        SystemUtils.Launch(new()
        {
            Local = PGColorMCPlugin.Config.ColorMC ?? "",
            Arg = $"-game {obj.UUID}"
        });
    }
}
