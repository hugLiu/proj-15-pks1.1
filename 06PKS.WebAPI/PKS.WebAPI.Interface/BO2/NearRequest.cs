using Newtonsoft.Json;
using PKS.Utils;

namespace PKS.WebAPI.Models
{
    [JsonObject(NamingStrategyType = typeof(LowerCaseNamingStrategy))]
    public class NearRequest
    {
        public string BOT { get; set; }
        public string BO { get; set; }
        public decimal Distince { get; set; }
        public int Top { get; set; }
    }
}