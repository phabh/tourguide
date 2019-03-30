using System;
using System.Threading.Tasks;
using TG.Api.Interfaces;
using TG.Api.Interfaces.Clients;
using TG.Api.Models;

namespace TG.Api.Services
{
    public class GoogleMapsService : IMapsService
    {
        private const string KEY = "AIzaSyDIPN1WOrn8WeATWKKmFTlQLuulgqX3jO4";

        private readonly IGoogleMapsClient _googleMapsClient;

        public GoogleMapsService(IGoogleMapsClient googleMapsClient)
        {
            _googleMapsClient = googleMapsClient;
        }

        public async Task<Candidate> GetPlacesResultAsync(string location, string filter)
        {
            try
            {
                var result = await _googleMapsClient.FindPlaceFromText(location, filter, "textquery", "place_id,name,geometry", KEY);

                if (!string.Equals(result.Status, "ok", StringComparison.InvariantCultureIgnoreCase) || result?.Candidates?.Length == 0)
                {
                    return null;
                }

                return result.Candidates[0];
            }
            catch
            {
                return null;
            }
        }
    }
}