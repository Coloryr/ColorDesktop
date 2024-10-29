using Newtonsoft.Json;

namespace ColorDesktop.ToDoPlugin.Objs;

public record ToDoListObj
{
    public record ValueObj
    {
        [JsonProperty("@odata.etag")]
        public string ETag { get; set; }
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
        [JsonProperty("isOwner")]
        public bool IsOwner { get; set; }
        [JsonProperty("isShared")]
        public bool IsShared { get; set; }
        [JsonProperty("wellknownListName")]
        public string WellknownListName { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
    }
    [JsonProperty("@odata.context")]
    public string Context { get; set; }
    public List<ValueObj> Value { get; set; }
}
