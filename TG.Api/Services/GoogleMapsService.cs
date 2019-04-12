using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Get name, address, price, rating, time and reviews for a place
        /// </summary>
        /// <param name="date"></param>
        /// <param name="placeId"></param>
        /// <returns></returns>
        public async Task<TourguideResponse> GetPlaceDetailsAsync(DateTime date, string placeId)
        {
            try
            {
                var result = await _googleMapsClient.GetPlaceDetails(placeId, KEY);

                var place = new TourguideResponse
                {
                    Name = result.Result.Name,
                    Address = result.Result.FormattedAddress,
                    Price = result.Result.PriceLevel.ToString(),
                    Rating = result.Result.Rating.ToString(),
                    Time = result.Result.OpeningHours.WeekdayText.Where(it => it.Contains(date.DayOfWeek.ToString())).FirstOrDefault().ToString(),
                    Review = ExtractReviewsFromResult(result)
                };

                return place;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private MyReview ExtractReviewsFromResult(PlaceDetails result)
        {
            MyReview review = new MyReview()
            {
                Comment = "",
                Rating = 0
            };

            MyReview greaterReview = new MyReview();

            foreach (Review r in result.Result.Reviews)
            {
                var tmpReview = new MyReview
                {
                    Comment = r.Text,
                    Rating = r.Rating
                };

                if (r.Rating > review.Rating && r.Text.Split().Length > 3)
                {
                    review = tmpReview;
                }
                else if (r.Text.Split().Length <= 3)
                {
                    greaterReview = tmpReview;
                }
            }

            if (review.Comment == "")
            {
                return greaterReview;
            }

            return review;
        }
    }
}