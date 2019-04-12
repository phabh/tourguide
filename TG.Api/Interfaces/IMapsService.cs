using System;
using System.Threading.Tasks;
using TG.Api.Enums;
using TG.Api.Models;

namespace TG.Api.Interfaces
{
    public interface IMapsService
    {
        Task<Candidate> GetPlacesResultAsync(string location, string filter);

        Task<Result[]> GetEstablishmentsAsync(string location, string keyWord, string minprice, string maxprice);

        Task<bool> IsOpenedAtDate(DateTime date, string placeId);

        Task<TourguideResponse> GetPlaceDetailsAsync(DateTime date, string placeId);
    }
}
