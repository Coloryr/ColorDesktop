using System.Text.Json.Serialization;

namespace ColorDesktop.BmPlugin;

public record BmObj
{
    public record WeekDayObj
    {
        [JsonPropertyName("en")]
        public string En { get; set; }
        [JsonPropertyName("cn")]
        public string Cn { get; set; }
        [JsonPropertyName("ja")]
        public string Ja { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
    public record ItemObj
    {
        public record RatingObj
        {
            public record CountObj
            {
                [JsonPropertyName("10")]
                public int C10 { get; set; }
                [JsonPropertyName("9")]
                public int C9 { get; set; }
                [JsonPropertyName("8")]
                public int C8 { get; set; }
                [JsonPropertyName("7")]
                public int C7 { get; set; }
                [JsonPropertyName("6")]
                public int C6 { get; set; }
                [JsonPropertyName("5")]
                public int C5 { get; set; }
                [JsonPropertyName("4")]
                public int C4 { get; set; }
                [JsonPropertyName("3")]
                public int C3 { get; set; }
                [JsonPropertyName("2")]
                public int C2 { get; set; }
                [JsonPropertyName("1")]
                public int C1 { get; set; }

            }
            [JsonPropertyName("total")]
            public int Total { get; set; }
            [JsonPropertyName("count")]
            public CountObj Count { get; set; }
            [JsonPropertyName("score")]
            public float Score { get; set; }
        }
        public record ImageObj
        {
            [JsonPropertyName("large")]
            public string Large { get; set; }
            [JsonPropertyName("common")]
            public string Common { get; set; }
            [JsonPropertyName("medium")]
            public string Medium { get; set; }
            [JsonPropertyName("small")]
            public string Small { get; set; }
            [JsonPropertyName("grid")]
            public string Grid { get; set; }
        }
        public record CollectionObj
        {
            [JsonPropertyName("doing")]
            public int Doing { get; set; }
        }
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("type")]
        public int Type { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("name_cn")]
        public string NameCn { get; set; }
        [JsonPropertyName("summary")]
        public string Summary { get; set; }
        [JsonPropertyName("air_date")]
        public string AirDate { get; set; }
        [JsonPropertyName("air_weekday")]
        public int AirWeekday { get; set; }
        [JsonPropertyName("rating")]
        public RatingObj Rating { get; set; }
        [JsonPropertyName("rank")]
        public int Rank { get; set; }
        [JsonPropertyName("images")]
        public ImageObj Images { get; set; }
        [JsonPropertyName("collection")]
        public CollectionObj Collection { get; set; }
    }

    [JsonPropertyName("weekday")]
    public WeekDayObj WeekDay { get; set; }
    [JsonPropertyName("items")]
    public List<ItemObj> Items { get; set; }
}
