using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ColorDesktop.BmPlugin;

public record BmObj
{
    public record WeekDayObj
    {
        [JsonProperty("en")]
        public string En { get; set; }
        [JsonProperty("cn")]
        public string Cn { get; set; }
        [JsonProperty("ja")]
        public string Ja { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
    }
    public record ItemObj
    {
        public record RatingObj
        {
            public record CountObj
            {
                [JsonProperty("10")]
                public int C10 { get; set; }
                [JsonProperty("9")]
                public int C9 { get; set; }
                [JsonProperty("8")]
                public int C8 { get; set; }
                [JsonProperty("7")]
                public int C7 { get; set; }
                [JsonProperty("6")]
                public int C6 { get; set; }
                [JsonProperty("5")]
                public int C5 { get; set; }
                [JsonProperty("4")]
                public int C4 { get; set; }
                [JsonProperty("3")]
                public int C3 { get; set; }
                [JsonProperty("2")]
                public int C2 { get; set; }
                [JsonProperty("1")]
                public int C1 { get; set; }

            }
            [JsonProperty("total")]
            public int Total { get; set; }
            [JsonProperty("count")]
            public CountObj Count { get; set; }
            [JsonProperty("score")]
            public float Score { get; set; }
        }
        public record ImageObj
        {
            [JsonProperty("large")]
            public string Large { get; set; }
            [JsonProperty("common")]
            public string Common { get; set; }
            [JsonProperty("medium")]
            public string Medium { get; set; }
            [JsonProperty("small")]
            public string Small { get; set; }
            [JsonProperty("grid")]
            public string Grid { get; set; }
        }
        public record CollectionObj
        {
            [JsonProperty("doing")]
            public int Doing { get; set; }
        }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("url")]
        public int Url { get; set; }
        [JsonProperty("type")]
        public int Type { get; set; }
        [JsonProperty("name")]
        public int Name { get; set; }
        [JsonProperty("name_cn")]
        public int NameCn { get; set; }
        [JsonProperty("summary")]
        public int Summary { get; set; }
        [JsonProperty("air_date")]
        public int AirDate { get; set; }
        [JsonProperty("air_weekday")]
        public int AirWeekday { get; set; }
        [JsonProperty("rating")]
        public RatingObj Rating { get; set; }
        [JsonProperty("rank")]
        public int Rank { get; set; }
        [JsonProperty("images")]
        public ImageObj Images { get; set; }
        [JsonProperty("collection")]
        public CollectionObj Collection { get; set; }
    }

    [JsonProperty("weekday")]
    public WeekDayObj WeekDay { get; set; }
    [JsonProperty("items")]
    public List<ItemObj> Items { get; set; }
}
