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
        private readonly PlaceType[] TravelPlan = new PlaceType[]
        {
            PlaceType.Cafe,
            PlaceType.Point_Of_Interest,
            PlaceType.Restaurant,
            PlaceType.Point_Of_Interest,
            PlaceType.Restaurant
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

        [HttpGet, Route("place/find")]
        public async Task<IActionResult> FindPlacesAsync([FromQuery] string geoLocation, [FromQuery] string date, [FromQuery] int price)
        {
            var allResults = new Dictionary<PlaceType, IEnumerable<Result>>();
            var realPlan = new List<Result>();

            foreach (var plan in TravelPlan)
            {
                IEnumerable<Result> results;

                if (allResults.ContainsKey(plan))
                {
                    results = allResults[plan];
                }
                else
                {
                    results = await _mapsService.GetEstablishmentsAsync(geoLocation, plan);

                    if (results is null)
                    {
                        return BadRequest();
                    }

                    allResults.Add(plan, results);
                }

                if (!DateTime.TryParse(date, out var datee))
                {
                    return BadRequest();
                }

                var correct = await FirstOrDefaultAsync(results, async r =>
                {
                    if (r.PriceLevel == price)
                    {
                        return await _mapsService.IsOpenedAtDate(datee, r.PlaceId);
                    }

                    return false;
                });
                
                if (correct == default)
                {
                    correct = results.FirstOrDefault();
                }

                allResults[plan] = allResults[plan].Where(r => r.Id != correct.Id);

                realPlan.Add(correct);
            }

            return Ok(realPlan);
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
