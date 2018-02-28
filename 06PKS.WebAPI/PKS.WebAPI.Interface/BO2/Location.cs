using System.Runtime.Serialization;

namespace PKS.WebAPI.Models
{
    public class Location
    {
        [DataMember(Name = "type", IsRequired = true)]
        public string Type { get; set; }

        [DataMember(Name = "coordinates", IsRequired = true)]
        public object[] Coordinates { get; set; }
    }
}