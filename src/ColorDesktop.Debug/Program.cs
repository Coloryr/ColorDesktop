using Avalonia;
using ColorDesktop.Api;
using ColorDesktop.PGLauncherPlugin.ColorMC;

namespace ColorDesktop.Debug;

internal class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => Launcher.Program.Main(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        new AnalogClockPlugin.AnalogClockPlugin().LoadLang(LanguageType.zh_cn);
        new BmPlugin.BmPlugin().LoadLang(LanguageType.zh_cn);
        new CalendarPlugin.CalendarPlugin().LoadLang(LanguageType.zh_cn);
        new ClockPlugin.ClockPlugin().LoadLang(LanguageType.zh_cn);
        new MonitorPlugin.MonitorPlugin().LoadLang(LanguageType.zh_cn);
        new OneWordPlugin.OneWordPlugin().LoadLang(LanguageType.zh_cn);
        new PGLauncherPlugin.PGLauncherPlugin().LoadLang(LanguageType.zh_cn);
        new PGColorMCPlugin().LoadLang(LanguageType.zh_cn);
        new WeatherPlugin.WeatherPlugin().LoadLang(LanguageType.zh_cn);
        //new Live2DPlugin.Live2DPlugin().LoadLang(LanguageType.zh_cn);
        //new MinecraftSkinPlugin.MinecraftSkinPlugin().LoadLang(LanguageType.zh_cn);
        new MinecraftMotdPlugin.MinecraftMotdPlugin().LoadLang(LanguageType.zh_cn);
        new ToDoPlugin.ToDoPlugin().LoadLang(LanguageType.zh_cn);
        new MusicControlPlugin.MusicControlPlugin().LoadLang(LanguageType.zh_cn);

        return Launcher.Program.BuildAvaloniaApp();
    }
}
