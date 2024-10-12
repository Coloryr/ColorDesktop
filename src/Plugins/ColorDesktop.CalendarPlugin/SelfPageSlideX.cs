using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.VisualTree;

namespace ColorDesktop.CalendarPlugin;

/// <summary>
/// Transitions between two pages by sliding them horizontally or vertically.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="SelfPageSlideY"/> class.
/// </remarks>
/// <param name="duration">The duration of the animation.</param>
/// <param name="orientation">The axis on which the animation should occur</param>
public class SelfPageSlideX(TimeSpan duration)
{
    /// <summary>
    /// Gets the duration of the animation.
    /// </summary>
    public TimeSpan Duration { get; set; } = duration;

    /// <summary>
    /// Gets or sets element entrance easing.
    /// </summary>
    public Easing SlideEasing { get; set; } = new CircularEaseInOut();

    /// <inheritdoc />
    public void Start(Visual? from, Visual? to, bool forward, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        var parent = GetVisualParent(from, to);
        var distance = parent.Bounds.Width;

        if (from != null)
        {
            double end = forward ? -distance : distance;
            var animation = new Animation
            {
                Easing = SlideEasing,
                FillMode = FillMode.Forward,
                Children =
                {
                    new KeyFrame
                    {
                        Setters =
                        {
                            new Setter
                            {
                                Property = TranslateTransform.XProperty,
                                Value = 0d
                            }
                        },
                        Cue = new Cue(0d)
                    },
                    new KeyFrame
                    {
                        Setters =
                        {
                            new Setter
                            {
                                Property = TranslateTransform.XProperty,
                                Value = end
                            }
                        },
                        Cue = new Cue(1d)
                    },
                    new KeyFrame
                    {
                        Setters =
                        {
                            new Setter
                            {
                                Property = Visual.IsVisibleProperty,
                                Value = false
                            }
                        },
                        Cue = new Cue(1d)
                    }
                },
                Duration = Duration
            };
            animation.RunAsync(from, cancellationToken);
        }

        if (to != null)
        {
            to.IsVisible = true;
            double end = forward ? distance : -distance;
            var animation = new Animation
            {
                FillMode = FillMode.Forward,
                Easing = SlideEasing,
                Children =
                {
                    new KeyFrame
                    {
                        Setters =
                        {
                            new Setter
                            {
                                Property = TranslateTransform.XProperty,
                                Value = end
                            }
                        },
                        Cue = new Cue(0d)
                    },
                    new KeyFrame
                    {
                        Setters =
                        {
                            new Setter
                            {
                                Property = TranslateTransform.XProperty,
                                Value = 0d
                            }
                        },
                        Cue = new Cue(1d)
                    }
                },
                Duration = Duration
            };
            animation.RunAsync(to, cancellationToken);
        }
    }
    protected static Visual GetVisualParent(Visual? from, Visual? to)
    {
        var p1 = (from ?? to)!.GetVisualParent();
        var p2 = (to ?? from)!.GetVisualParent();

        if (p1 != null && p2 != null && p1 != p2)
        {
            throw new ArgumentException("Controls for PageSlide must have same parent.");
        }

        return p1 ?? throw new InvalidOperationException("Cannot determine visual parent.");
    }
}