using System.Threading.Tasks;
using TG.Api.Models;

namespace TG.Api.Interfaces
{
    public interface IMapsService
    {
        Task<Candidate> GetPlacesResultAsync(string location, string filter);
    }
}
