using Newtonsoft.Json;
using TG.Api.Enums;

namespace TG.Api.Models
{
    public class Establishments
    {
        public string Status { get; set; }

        public Result[] Results { get; set; }
    }

    public class Result
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("place_id")]
        public string PlaceId { get; set; }

        [JsonProperty("rating")]
        public double Rating { get; set; }

        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }

        [JsonProperty("price_level")]
        public uint PriceLevel { get; set; }

        [JsonProperty("user_ratings_total")]
        public int UserRatingsTotal { get; set; }

        public PlaceType PlaceType { get; set; }
    }
}
