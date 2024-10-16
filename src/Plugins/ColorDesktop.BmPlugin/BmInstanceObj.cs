namespace ColorDesktop.BmPlugin;

public enum SkinType
{
    Skin1, Skin2
}

public record BmInstanceObj
{
    public SkinType Skin { get; set; }
}
