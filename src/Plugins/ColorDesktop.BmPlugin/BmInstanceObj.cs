namespace ColorDesktop.BmPlugin;

public enum SkinType
{
    Skin1, Skin2, Skin3
}

public record BmInstanceObj
{
    public SkinType Skin { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string Color1 { get; set; }
    public string Color2 { get; set; }
    public int Width1 { get; set; }
}
