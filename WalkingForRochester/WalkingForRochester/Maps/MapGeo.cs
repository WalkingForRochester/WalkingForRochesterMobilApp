using System.Collections.Generic;
using Newtonsoft.Json;

namespace WalkingForRochester.Maps
{
    public class MapGeo
    {
        [JsonProperty("Coordinates")] public List<Coordinate> Coordinates { get; set; }

        public class Coordinate
        {
            [JsonProperty("Longitude")] public double Longitude { get; set; }
            [JsonProperty("Latitude")] public double Latitude { get; set; }
        }
    }
}