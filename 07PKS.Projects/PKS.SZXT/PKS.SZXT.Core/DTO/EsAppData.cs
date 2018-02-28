using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PKS.SZXT.Core.DTO
{
    public class EsAppData : Dictionary<string, object>
    {
        public EsAppData() { }
        [JsonIgnore]
        public object ShowType
        {
            get { return base[nameof(ShowType)]; }
            set { base[nameof(ShowType)] = value; }
        }
        [JsonIgnore]
        public object Content
        {
            get { return base[nameof(Content)]; }
            set { base[nameof(Content)] = value; }
        }
        [JsonIgnore]
        public object Url
        {
            get { return base[nameof(Url)]; }
            set { base[nameof(Url)] = value; }
        }
    }
}
