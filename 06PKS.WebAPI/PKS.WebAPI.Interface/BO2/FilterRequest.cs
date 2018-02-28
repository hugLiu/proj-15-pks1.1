using Newtonsoft.Json.Linq;
using PKS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PKS.WebAPI.Models
{
    public class FilterRequest
    {
        public FilterRequest()
        {
            this.Query = new JObject();
            this.Fields = new JObject();
            this.Sort = new JObject();
        }

        [DataMember(Name = "query", IsRequired = false)]
        public object Query { get; set; }

        [DataMember(Name = "fields", IsRequired = false)]
        public object Fields { get; set; }

        [DataMember(Name = "sort", IsRequired = false)]
        public object Sort { get; set; }

        [DataMember(Name = "skip", IsRequired = false)]
        public int? Skip { get; set; }

        [DataMember(Name = "limit", IsRequired = false)]
        public int? Limit { get; set; }
    }
}
