using Newtonsoft.Json;

namespace TG.Api.Models
{
    public class Place
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("candidates")]
        public Candidate[] Candidates { get; set; }
    }

    public class Candidate
    {
        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("place_id")]
        public string PlaceId { get; set; }
    }
}