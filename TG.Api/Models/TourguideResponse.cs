using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TG.Api.Models
{
    public class TourguideResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("rating")]
        public string Rating { get; set; }

        [JsonProperty("reviews")]
        public MyReview Review { get; set; }

        [JsonProperty("skip")]
        public long Skip { get; set; }
    }

    public class MyReview
    {
        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("rating")]
        public long Rating { get; set; }
    }
}
