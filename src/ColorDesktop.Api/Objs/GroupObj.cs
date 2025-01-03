namespace ColorDesktop.Api.Objs;

public class GroupObj
{
    public string Name { get; set; }
    public string UUID { get; set; }
    public HashSet<string> Instances { get; set; }
    public HashSet<string> Enables { get; set; }
}
