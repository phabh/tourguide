using Newtonsoft.Json;
using TG.Api.Enums;

namespace TG.Api.Models
{
    public class Establishments
    {
        public string Status { get; set; }

        public Result[] Results { get; set; }
    }
}
