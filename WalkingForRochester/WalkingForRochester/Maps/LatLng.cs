using System.Collections.Generic;

namespace WalkingForRochester.Maps
{
    public class Location
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class Geometry
    {
        public Location Location { get; set; }
    }

    public class Result
    {
        public Geometry Geometry { get; set; }
    }

    public class LatLng
    {
        public List<Result> Results { get; set; }
    }
}