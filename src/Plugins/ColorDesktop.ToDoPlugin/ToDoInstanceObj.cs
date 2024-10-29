namespace ColorDesktop.ToDoPlugin;

public record ToDoInstanceObj
{
    public int Width { get; set; }
    public int Height { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public string BackColor { get; set; }
    public string TextColor { get; set; }
}
