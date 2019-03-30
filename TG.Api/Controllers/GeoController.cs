using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        public async Task<IActionResult> FindPlacesAsync([FromQuery] string geoLocation, [FromQuery] string date, [FromQuery] string price)
        {
            var allResults = new Dictionary<PlaceType, Result[]>();
            
            foreach (var plan in TravelPlan)
            {
                Result[] results;

                if (allResults.ContainsKey(plan))
                {
                    results = allResults[plan];
                }
                else
                {
                    results = await _mapsService.GetEstablishmentsAsync(geoLocation, plan);
                    allResults.Add(plan, results);
                }


            }
            // Para cada travel plan -->
            //     Pega dados e poe no dicionario
            //     Faz match por preço e data e tira do dicionario
            //     Adiciona ao travel guide
            // Retorna
        }
    }
}
