using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PKS.SZZSK.Web.Models
{
    public class TemplateUrl
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "pageid")]
        public int PageId { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "instanceclass")]
        public string InstanceClass { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }
    }
}