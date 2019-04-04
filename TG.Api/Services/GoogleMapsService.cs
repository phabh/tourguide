using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading.Tasks;
using TG.Api.Enums;
using TG.Api.Interfaces;
using TG.Api.Interfaces.Clients;
using TG.Api.Models;

namespace TG.Api.Services
{
    public class GoogleMapsService : IMapsService
    {
        private const string KEY = "AIzaSyDIPN1WOrn8WeATWKKmFTlQLuulgqX3jO4";
        private const int TIME = 100;

        private readonly IGoogleMapsClient _googleMapsClient;
        private readonly IMemoryCache _googleMapsCache;

        public GoogleMapsService(IGoogleMapsClient googleMapsClient, IMemoryCache googleMapsCache)
        {
            _googleMapsClient = googleMapsClient;
            _googleMapsCache = googleMapsCache;
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
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Get the places of a neighborhood
        /// </summary>
        /// <param name="location"></param>
        /// <param name="keyWord"></param>
        /// <param name="minprice"></param>
        /// <param name="maxprice"></param>
        /// <returns></returns>
        public async Task<Result[]> GetEstablishmentsAsync(string location, string keyWord, string minprice, string maxprice)
        {
            try
            {
                var cacheKey = location.ToString()+keyWord.ToString()+minprice.ToString()+maxprice.ToString();

                var result = await _googleMapsCache.GetOrCreateAsync(cacheKey, entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromSeconds(TIME);
                    return _googleMapsClient.FindNearbyPlaces(location, 20000, keyWord, minprice, maxprice, KEY);
                });

                if (!string.Equals(result.Status, "ok", StringComparison.InvariantCultureIgnoreCase) || result?.Results?.Length == 0)
                {
                    return null;
                }

                return result.Results;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> IsOpenedAtDate(DateTime date, string placeId)
        {
            try
            {
                var result = await _googleMapsClient.GetPlaceDetails(placeId, "opening_hours", KEY);

                if (!string.Equals(result.Status, "ok", StringComparison.InvariantCultureIgnoreCase) || result?.Result?.OpeningHours?.Periods?.Length == 0)
                {
                    return false;
                }

                return result.Result.OpeningHours.Periods.Any(d => d.Open.Day == (long)date.DayOfWeek);
            }
            catch
            {
                return false;
            }
        }
    }
}