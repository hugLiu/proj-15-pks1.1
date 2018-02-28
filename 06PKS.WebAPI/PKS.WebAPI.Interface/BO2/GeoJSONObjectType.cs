using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PKS.WebAPI.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum GeoJSONObjectType
    {
        Point,
        Polygon,
        LineString,
        MultiPoint,
        MultiLineString,
        MultiPolygon,
        GeometryCollection
    }
}
