using System.Text.Json.Serialization;

namespace ColorDesktop.ToDoPlugin.Objs;

public record ToDoListObj
{
    public record ValueObj
    {
        [JsonPropertyName("@odata.etag")]
        public string ETag { get; set; }
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }
        [JsonPropertyName("isOwner")]
        public bool IsOwner { get; set; }
        [JsonPropertyName("isShared")]
        public bool IsShared { get; set; }
        [JsonPropertyName("wellknownListName")]
        public string WellknownListName { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
    [JsonPropertyName("@odata.context")]
    public string Context { get; set; }
    public List<ValueObj> Value { get; set; }
}
