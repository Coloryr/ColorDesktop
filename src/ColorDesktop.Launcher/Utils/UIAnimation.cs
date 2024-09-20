using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Styling;

namespace ColorDesktop.Launcher.Utils;

public static class UIAnimation
{
    public static readonly Animation ShowAnimation = new Animation
    {
        Duration = TimeSpan.FromMilliseconds(300),
        Children =
            {
                new KeyFrame
                {
                    Cue = new Cue(0),
                    Setters =
                    {
                        new Setter(Visual.OpacityProperty, 0)
                    }
                },
                new KeyFrame
                {
                    Cue = new Cue(1),
                    Setters =
                    {
                        new Setter(Visual.OpacityProperty, 1)
                    }
                }
            }
    };

    public static readonly Animation HideAnimation = new Animation
    {
        Duration = TimeSpan.FromMilliseconds(300),
        Children =
            {
                new KeyFrame
                {
                    Cue = new Cue(0),
                    Setters =
                    {
                        new Setter(Visual.OpacityProperty, 1)
                    }
                },
                new KeyFrame
                {
                    Cue = new Cue(1),
                    Setters =
                    {
                        new Setter(Visual.OpacityProperty, 0)
                    }
                }
            }
    };
}
