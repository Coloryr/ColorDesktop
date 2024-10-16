﻿using System;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Styling;

namespace ColorDesktop.Launcher.Utils;

public static class UIAnimation
{
    public static readonly Animation ShowAnimation = new()
    {
        Duration = TimeSpan.FromMilliseconds(300),
        Children =
        {
            new KeyFrame
            {
                Cue = new Cue(0),
                Setters =
                {
                    new Setter(Visual.OpacityProperty, 0.0d)
                }
            },
            new KeyFrame
            {
                Cue = new Cue(1),
                Setters =
                {
                    new Setter(Visual.OpacityProperty, 1.0d)
                }
            }
        }
    };

    public static readonly Animation HideAnimation = new()
    {
        Duration = TimeSpan.FromMilliseconds(300),
        Children =
        {
            new KeyFrame
            {
                Cue = new Cue(0),
                Setters =
                {
                    new Setter(Visual.OpacityProperty, 1.0d)
                }
            },
            new KeyFrame
            {
                Cue = new Cue(1),
                Setters =
                {
                    new Setter(Visual.OpacityProperty, 0.0d)
                }
            }
        }
    };
}
