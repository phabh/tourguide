using RestEase;
using System.Threading.Tasks;
using TG.Api.Models;

namespace TG.Api.Interfaces.Clients
{
    public interface IGoogleMapsClient
    {
        [Get("/place/findplacefromtext/json")]
        Task<Place> FindPlaceFromText([Query("input")] string text, [Query("locationbias")] string locationbias, [Query("inputtype")] string inputType, [Query("fields")] string fields, [Query("key")] string key);

        [Get("/place/nearbysearch/json")]
        Task<Establishments> FindNearbyPlaces([Query("location")] string location, [Query("radius")] int radius, [Query("keyword")] string keyworkd, [Query("minprice")] string minprice, [Query("maxprice")] string maxprice, [Query("key")] string key);

        [Get("/place/details/json")]
        Task<PlaceDetails> GetPlaceDetails([Query("placeid")] string placeId, [Query("fields")] string fields, [Query("key")] string key);
    }
}
