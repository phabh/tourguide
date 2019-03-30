using RestEase;
using System.Threading.Tasks;
using TG.Api.Models;

namespace TG.Api.Interfaces.Clients
{
    public interface IGoogleMapsClient
    {
        [Get("/place/findplacefromtext/json")]
        Task<Place> FindPlaceFromText([Query("input")] string text, [Query("locationbias")] string locationbias, [Query("inputtype")] string inputType, [Query("fields")] string fields, [Query("key")] string key);
    }
}
