using Newtonsoft.Json;
using PKS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.WebAPI.Models
{
    [JsonObject(NamingStrategyType = typeof(LowerCaseNamingStrategy))]
    public class SimpleSearchResult
    {
        public long Took { get; set; }
        public long Total { get; set; }
        public List<object> Results { get; set; }
    }
}
