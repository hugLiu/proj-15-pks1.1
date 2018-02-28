using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.WebAPI.Models
{
    public class GeoPolygon:Location
    {
        public GeoPolygon(List<List<Point>> points)
        {
            this.Type = "Polygon";
            this.Coordinates = new object[points.Count][];

            for(int i=0;i<points.Count; i++)
            {
                this.Coordinates[i] = new object[points[i].Count];
                for(int j=0;j<points[i].Count;j++)
                {
                    Point point = points[i][j];
                    ((object[])this.Coordinates[i])[j] = new object[2] { point.Longitude, point.Latitude };
                }
            }
        }
    }
}
