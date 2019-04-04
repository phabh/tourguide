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
            { PlaceType.PointOfInterest, new [] { "museum", "park" } },
            { PlaceType.Lunch, new [] { "lunch", "restaurant" } }
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
        /// Returns the results for an specified type - coffe, PointOfInterest, Lunch, Dinner
        /// </summary>
        /// <param name="geolocation"></param>
        /// <param name="date"></param>
        /// <param name="price"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet, Route("place/find/{type}/{skip}")]
        public async Task<IActionResult> FindPlaceAsync([FromQuery] string geolocation, [FromQuery] string date, [FromQuery] string minprice, [FromQuery] string maxprice, [FromRoute] string type, [FromRoute] int skip)
        {
            try
            {
                var results = new List<Result>();
                var dateTime = Convert.ToDateTime(date);
                string[] keyWordList;
                keyWords.TryGetValue((PlaceType)Enum.Parse(typeof(PlaceType), type, true), out keyWordList);

                for (int i = 0; i < keyWordList.Length; i++)
                {
                    var response = await _mapsService.GetEstablishmentsAsync(geolocation, keyWordList[i], minprice, maxprice);
                    if (response == null) continue;
                    results.AddRange(response);
                }

                if (!results.Any())
                {
                    return NoContent();
                }

                foreach (Result result in results.Skip(skip))
                {
                    bool isOpened = await _mapsService.IsOpenedAtDate(dateTime, result.PlaceId);
                    if (isOpened)
                    {
                        result.Skip = results.IndexOf(result);
                        return Ok(result);
                    }
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        private async Task<T> FirstOrDefaultAsync<T>(IEnumerable<T> source, Func<T, Task<bool>> predicate)
        {
            foreach (var i in source)
            {
                if (await predicate(i))
                {
                    return i;
                }
            }

            return default;
        }
    }
}
