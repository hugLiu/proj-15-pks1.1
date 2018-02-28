using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.WebAPI.Models
{
    public class GeoPoint:Location
    {
        public GeoPoint(decimal longitude,decimal latitude)
        {
            this.Type = "Point";
            this.Coordinates = new object[] { longitude, latitude };
        }

        public GeoPoint(Point point):this(point.Latitude,point.Latitude)
        {

        }
    }
}
