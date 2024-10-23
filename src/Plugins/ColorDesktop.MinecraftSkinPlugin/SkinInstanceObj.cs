using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinecraftSkinRender;

namespace ColorDesktop.MinecraftSkinPlugin;

public enum FileType
{ 
    Name, Url, LocalFile
}

public record SkinInstanceObj
{
    public int Width { get; set; }
    public int Height { get; set; }
    public bool LowFps { get; set; }
    public bool DisplayFps { get; set; }
    public bool EnableMSAA { get; set; }
    public FileType FileType { get; set; }
    public SkinType SkinType { get; set; }
    public string File { get; set; }
    public string File1 { get; set; }
    public bool EnableTop { get; set; }
    public bool EnableCape { get; set; }
    public bool EnableAnimation { get; set; }
}
