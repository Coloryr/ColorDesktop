using Newtonsoft.Json;

namespace ColorDesktop.ToDoPlugin.Objs;

public record ToDoTaskListObj
{
    public record ValueObj
    {
        public record BodyObj
        {
            [JsonProperty("content")]
            public string Content { get; set; }
            [JsonProperty("contentType")]
            public string ContentType { get; set; }
        }
        public record TimeObj
        {
            [JsonProperty("dateTime")]
            public DateTime DateTime { get; set; }
            [JsonProperty("timeZone")]
            public string TimeZone { get; set; }
        }
        public record CheckListObj
        {
            [JsonProperty("displayName")]
            public string DisplayName { get; set; }
            [JsonProperty("createdDateTime")]
            public DateTime CreatedDateTime { get; set; }
            [JsonProperty("checkedDateTime")]
            public DateTime CheckedDateTime { get; set; }
            [JsonProperty("isChecked")]
            public bool IsChecked { get; set; }
            [JsonProperty("id")]
            public string Id { get; set; }
        }
        [JsonProperty("@odata.etag")]
        public string ETag { get; set; }
        [JsonProperty("importance")]
        public string Importance { get; set; }
        [JsonProperty("isReminderOn")]
        public bool IsReminderOn { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }
        [JsonProperty("lastModifiedDateTime")]
        public DateTime LastModifiedDateTime { get; set; }
        [JsonProperty("hasAttachments")]
        public bool HasAttachments { get; set; }
        [JsonProperty("categories")]
        public List<object> Categories { get; set; }
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("body")]
        public BodyObj Body { get; set; }
        [JsonProperty("dueDateTime")]
        public TimeObj? DueDateTime { get; set; }
        [JsonProperty("reminderDateTime")]
        public TimeObj? ReminderDateTime { get; set; }
        [JsonProperty("completedDateTime")]
        public TimeObj? CompletedDateTime { get; set; }
        [JsonProperty("checklistItems@odata.context")]
        public string? Context { get; set; }
        [JsonProperty("checklistItems")]
        public List<CheckListObj>? ChecklistItems { get; set; }
    }
    [JsonProperty("@odata.context")]
    public string Context { get; set; }
    [JsonProperty("value")]
    public List<ValueObj> Value { get; set; }
}
