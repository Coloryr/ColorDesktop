namespace ColorDesktop.PluginList;

public record PluginDownloadObj
{
    public record ItemObj
    {
        public record FileObj
        {
            public string Name { get; set; }
            public string Sha1 { get; set; }
        }
        public string ID { get; set; }
        public string Name { get; set; }
        public string Describe { get; set; }
        public string Auther { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Version { get; set; }
        public List<string> Deps { get; set; }
        public List<FileObj> Files { get; set; }
        public List<string> Os { get; set; }
        public string ApiVersion { get; set; }
    }
    public string Source { get; set; }
    public string BaseUrl { get; set; }
    public List<ItemObj> Plugins { get; set; }
}
