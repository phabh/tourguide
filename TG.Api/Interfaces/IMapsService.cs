using System.Threading.Tasks;
using TG.Api.Enums;
using TG.Api.Models;

namespace TG.Api.Interfaces
{
    public interface IMapsService
    {
        Task<Candidate> GetPlacesResultAsync(string location, string filter);

        Task<Result[]> GetEstablishmentsAsync(string location, PlaceType placeType);
    }
}
