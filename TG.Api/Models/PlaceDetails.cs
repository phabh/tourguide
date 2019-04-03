using Newtonsoft.Json;

namespace TG.Api.Models
{
    public class PlaceDetails
    {
        [JsonProperty("result")]
        public PlaceDetailsResult Result { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }


    public class PlaceDetailsResult
    {
        [JsonProperty("opening_hours")]
        public OpeningHours OpeningHours { get; set; }
    }

    public class OpeningHours
    {
        [JsonProperty("open_now")]
        public bool OpenNow { get; set; }

        [JsonProperty("periods")]
        public Period[] Periods { get; set; }

        [JsonProperty("weekday_text")]
        public string[] WeekdayText { get; set; }
    }

    public class Period
    {
        [JsonProperty("close")]
        public Close Close { get; set; }

        [JsonProperty("open")]
        public Close Open { get; set; }
    }

    public class Close
    {
        [JsonProperty("day")]
        public long Day { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }
    }

}
