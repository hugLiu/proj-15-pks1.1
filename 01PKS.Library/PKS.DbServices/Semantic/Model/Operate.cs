using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices.Semantic.Model
{
    [Serializable]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Operate
    {
        
        Create = 1,
        Update = 2,
        Delete = 3
    }
}
