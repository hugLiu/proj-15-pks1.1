using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.WebAPI.Models
{
    public class Point
    {
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }

        public Point(decimal longitude, decimal latitude)
        {
            this.Longitude = longitude;
            this.Latitude = latitude;
        }
    }
}
