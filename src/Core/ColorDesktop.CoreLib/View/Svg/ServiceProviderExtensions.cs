using Avalonia.Markup.Xaml;

namespace ColorDesktop.CoreLib.View.Svg;

public static class ServiceProviderExtensions
{
    public static T GetService<T>(this IServiceProvider sp)
        => (T)sp?.GetService(typeof(T))!;

    public static Uri GetContextBaseUri(this IServiceProvider ctx)
        => ctx.GetService<IUriContext>().BaseUri;
}