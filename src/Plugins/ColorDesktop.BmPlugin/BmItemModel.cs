using Avalonia.Media;
using Avalonia.Media.Immutable;
using ColorDesktop.CoreLib;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorDesktop.BmPlugin;

public partial class BmItemModel(BmObj.ItemObj item) : ObservableObject
{
    public string Text => string.IsNullOrWhiteSpace(item.NameCn) ? item.Name : item.NameCn;

    public string Rating => $"{item.Rating?.Score ?? 0:0.0}";

    public IBrush RatingColor => GetIntermediateColor(Colors.Red, Color.Parse("#00FF00"), item.Rating?.Score / 10);

    public void OpenUrl()
    {
        CoreHelper.OpUrl(item.Url);
    }

    private static IBrush GetIntermediateColor(Color colorA, Color colorB, float? percentage)
    {
        if (percentage == null)
        {
            return Brushes.Black;
        }
        // 确保 percentage 在 0 到 1 之间
        if (percentage < 0 || percentage > 1)
            throw new ArgumentOutOfRangeException(nameof(percentage), "Percentage must be between 0 and 1.");

        var hsv = new HsvColor(1, 120d * percentage.Value, 1, 1);

        return new ImmutableSolidColorBrush(hsv.ToRgb());
    }
}
