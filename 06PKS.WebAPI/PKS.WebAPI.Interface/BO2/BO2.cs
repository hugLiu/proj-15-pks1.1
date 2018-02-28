using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PKS.WebAPI.Models
{
    public class BO2
    {

        [DataMember(Name = "boid", IsRequired = true)]
        public string BOID { get; set; }

        [DataMember(Name = "bo", IsRequired = true)]
        public string BO { get; set; }

        [DataMember(Name = "alias", IsRequired = false)]
        public string[] Alias { get; set; }

        [DataMember(Name = "bot", IsRequired = true)]
        public string BOT { get; set; }

        [DataMember(Name = "properties", IsRequired = false)]
        public PropertyCollection Properties { get; set; }

        [DataMember(Name = "location", IsRequired = false)]
        public object Location { get; set; }
    }
}