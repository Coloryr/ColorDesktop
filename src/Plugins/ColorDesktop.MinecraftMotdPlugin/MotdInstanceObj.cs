namespace ColorDesktop.MinecraftMotdPlugin;

public record MotdInstanceObj
{
    public string IP { get; set; }
    public ushort? Port { get; set; }
}
