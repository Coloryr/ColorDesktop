using System.Text.Json.Serialization;

namespace ColorDesktop.ToDoPlugin.Objs;

public record ToDoTaskListObj
{
    public record ValueObj1
    {
        public record BodyObj
        {
            [JsonPropertyName("content")]
            public string Content { get; set; }
            [JsonPropertyName("contentType")]
            public string ContentType { get; set; }
        }
        public record TimeObj
        {
            [JsonPropertyName("dateTime")]
            public DateTime DateTime { get; set; }
            [JsonPropertyName("timeZone")]
            public string TimeZone { get; set; }
        }
        public record CheckListObj
        {
            [JsonPropertyName("displayName")]
            public string DisplayName { get; set; }
            [JsonPropertyName("createdDateTime")]
            public DateTime CreatedDateTime { get; set; }
            [JsonPropertyName("checkedDateTime")]
            public DateTime CheckedDateTime { get; set; }
            [JsonPropertyName("isChecked")]
            public bool IsChecked { get; set; }
            [JsonPropertyName("id")]
            public string Id { get; set; }
        }
        [JsonPropertyName("@odata.etag")]
        public string ETag { get; set; }
        [JsonPropertyName("importance")]
        public string Importance { get; set; }
        [JsonPropertyName("isReminderOn")]
        public bool IsReminderOn { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }
        [JsonPropertyName("lastModifiedDateTime")]
        public DateTime LastModifiedDateTime { get; set; }
        [JsonPropertyName("hasAttachments")]
        public bool HasAttachments { get; set; }
        [JsonPropertyName("categories")]
        public List<object> Categories { get; set; }
        [JsonPropertyName("id")]
        public string ID { get; set; }
        [JsonPropertyName("body")]
        public BodyObj Body { get; set; }
        [JsonPropertyName("dueDateTime")]
        public TimeObj? DueDateTime { get; set; }
        [JsonPropertyName("reminderDateTime")]
        public TimeObj? ReminderDateTime { get; set; }
        [JsonPropertyName("completedDateTime")]
        public TimeObj? CompletedDateTime { get; set; }
        [JsonPropertyName("checklistItems@odata.context")]
        public string? Context { get; set; }
        [JsonPropertyName("checklistItems")]
        public List<CheckListObj>? ChecklistItems { get; set; }
    }
    [JsonPropertyName("@odata.context")]
    public string Context { get; set; }
    [JsonPropertyName("value")]
    public List<ValueObj1> Value { get; set; }
}
