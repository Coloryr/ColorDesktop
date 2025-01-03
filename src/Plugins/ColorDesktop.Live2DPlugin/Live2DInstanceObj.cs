﻿namespace ColorDesktop.Live2DPlugin;

public record Live2DModelObj
{
    public string Name { get; set; }
    public string Local { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public float Scale { get; set; }
}

public record Live2DInstanceObj
{
    public List<Live2DModelObj> Models { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public bool LowFps { get; set; }
    public bool DisplayFps { get; set; }
}
