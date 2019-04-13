using Newtonsoft.Json;
using System;

namespace TG.Api.Models
{
    public partial class PlaceDetails
    {
        [JsonProperty("html_attributions")]
        public object[] HtmlAttributions { get; set; }

        [JsonProperty("result")]
        public Result Result { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("address_components")]
        public AddressComponent[] AddressComponents { get; set; }

        [JsonProperty("adr_address")]
        public string AdrAddress { get; set; }

        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }

        [JsonProperty("formatted_phone_number")]
        public string FormattedPhoneNumber { get; set; }

        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }

        [JsonProperty("price_level")]
        public uint PriceLevel { get; set; }

        [JsonProperty("icon")]
        public Uri Icon { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("international_phone_number")]
        public string InternationalPhoneNumber { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("opening_hours")]
        public OpeningHours OpeningHours { get; set; }

        [JsonProperty("photos")]
        public Photo[] Photos { get; set; }

        [JsonProperty("place_id")]
        public string PlaceId { get; set; }

        [JsonProperty("plus_code")]
        public PlusCode PlusCode { get; set; }

        [JsonProperty("rating")]
        public double Rating { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("reviews")]
        public Review[] Reviews { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("types")]
        public string[] Types { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("user_ratings_total")]
        public long UserRatingsTotal { get; set; }

        [JsonProperty("utc_offset")]
        public long UtcOffset { get; set; }

        [JsonProperty("vicinity")]
        public string Vicinity { get; set; }

        [JsonProperty("website")]
        public Uri Website { get; set; }
    }

    public partial class AddressComponent
    {
        [JsonProperty("long_name")]
        public string LongName { get; set; }

        [JsonProperty("short_name")]
        public string ShortName { get; set; }

        [JsonProperty("types")]
        public string[] Types { get; set; }
    }

    public partial class Geometry
    {
        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("viewport")]
        public Viewport Viewport { get; set; }
    }

    public partial class Location
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lng")]
        public double Lng { get; set; }
    }

    public partial class Viewport
    {
        [JsonProperty("northeast")]
        public Location Northeast { get; set; }

        [JsonProperty("southwest")]
        public Location Southwest { get; set; }
    }

    public partial class OpeningHours
    {
        [JsonProperty("open_now")]
        public bool OpenNow { get; set; }

        [JsonProperty("periods")]
        public Period[] Periods { get; set; }

        [JsonProperty("weekday_text")]
        public string[] WeekdayText { get; set; }
    }

    public partial class Period
    {
        [JsonProperty("close")]
        public Close Close { get; set; }

        [JsonProperty("open")]
        public Close Open { get; set; }
    }

    public partial class Close
    {
        [JsonProperty("day")]
        public long Day { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }
    }

    public partial class Photo
    {
        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("html_attributions")]
        public string[] HtmlAttributions { get; set; }

        [JsonProperty("photo_reference")]
        public string PhotoReference { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }
    }

    public partial class PlusCode
    {
        [JsonProperty("compound_code")]
        public string CompoundCode { get; set; }

        [JsonProperty("global_code")]
        public string GlobalCode { get; set; }
    }

    public partial class Review
    {
        [JsonProperty("author_name")]
        public string AuthorName { get; set; }

        [JsonProperty("author_url")]
        public Uri AuthorUrl { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("profile_photo_url")]
        public Uri ProfilePhotoUrl { get; set; }

        [JsonProperty("rating")]
        public long Rating { get; set; }

        [JsonProperty("relative_time_description")]
        public string RelativeTimeDescription { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }
    }
}