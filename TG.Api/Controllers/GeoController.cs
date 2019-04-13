using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using TG.Api.Enums;
using TG.Api.Interfaces;
using TG.Api.Models;

namespace TG.Api.Controllers
{
    [ApiController, Produces(MediaTypeNames.Application.Json), Route("[controller]")]
    public class GeoController : ControllerBase
    {
        private readonly Dictionary<PlaceType, string[]> keyWords = new Dictionary<PlaceType, string[]>
        {
            { PlaceType.Coffee, new [] { "coffee" } },
            { PlaceType.PointOfInterest, new [] { "museum", "park", "exhibition", "waterfall", "statue", "monument", "culture", "lake", "oscar niemeyer", "garden", "zoo", "sunset", "montain", "cavern", "river", "nature reserve" } },
            { PlaceType.Lunch, new [] { "lunch", "restaurant" } },
            { PlaceType.Dinner, new [] { "dinner", "restaurant" } }
        };

        private readonly IMapsService _mapsService;

        public GeoController(IMapsService mapsService)
        {
            _mapsService = mapsService;
        }

        [HttpGet, Route("place/info")]
        public async Task<IActionResult> GetInformationAsync([FromQuery] string location, [FromQuery] string filter)
        {
            var result = await _mapsService.GetPlacesResultAsync(location, filter);

            if (result is null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }

        /// <summary>
        /// Returns the results for an specified type - coffe, PointOfInterest, Lunch, Dinner.
        /// </summary>
        /// <param name="geolocation">The coordinates in google maps format </param>
        /// <param name="date">Day of tourguide - dd/mm/yyyy </param>
        /// <param name="minprice">The minimum price that the user wants to pay, 0 (cheapest) - 4 (most expensive)</param>
        /// <param name="maxprice">The minimum price that the user wants to pay, 0 (cheapest) - 4 (most expensive)</param>
        /// <param name="type">The type of search (Coffee, PointOfInterest, Lunch, Dinner)</param>
        /// <param name="skip">The index to initialize the search in google results</param>
        /// <returns>Result with the first valid ocurrence and the skip indicating the index of this ocurrence</returns>
        [HttpGet, Route("place/find/{type}/{skip}")]
        public async Task<IActionResult> FindPlaceAsync([FromQuery] string geolocation, [FromQuery] string date, [FromQuery] string minprice, [FromQuery] string maxprice, [FromRoute] string type, [FromRoute] int skip)
        {
            try
            {
                var dateTime = Convert.ToDateTime(date);
                keyWords.TryGetValue((PlaceType)Enum.Parse(typeof(PlaceType), type, true), out string[] keyWordList);

                var list = keyWordList.ToList();

                var tmpResult = await Task.WhenAll(list.Select(it => _mapsService.GetEstablishmentsAsync(geolocation, it, minprice, maxprice)));
                tmpResult = tmpResult.Where(it => it != null).ToArray();
                var results = tmpResult.SelectMany(it => it).ToList();

                if (results.Count == 0)
                {
                    return NoContent();
                }

                foreach (Result res in results.Skip(skip))
                {
                    bool isOpened = await _mapsService.IsOpenedAtDate(dateTime, res.PlaceId);
                    if (isOpened)
                    {
                        var place = await _mapsService.GetPlaceDetailsAsync(dateTime, res.PlaceId);
                        place.Skip = results.IndexOf(res);
                        return Ok(place);
                    }
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}