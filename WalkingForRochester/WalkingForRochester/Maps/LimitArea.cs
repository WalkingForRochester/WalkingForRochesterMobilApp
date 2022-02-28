using System.Threading.Tasks;
using WalkingForRochester.Services;
using Xamarin.Forms.Maps;

namespace WalkingForRochester.Maps
{
    public class LimitArea
    {
        public static async Task<bool> AreaWithin(Position p)
        {
            var mapData = await RestService.Resources.GetMapData();
            var coordinates = mapData.Coordinates;
            var c = false;
            var i = 0;
            var j = coordinates.Count - 1;
            foreach (var point in coordinates)
            {
                var lat_i = point.Latitude;
                var lon_i = point.Longitude;
                var lat_j = coordinates[j].Latitude;
                var lon_j = coordinates[j].Longitude;

                if (lat_i > p.Latitude != lat_j > p.Latitude &&
                    p.Longitude < (lon_j - lon_i) * (p.Latitude - lat_i) / (lat_j - lat_i) + lon_i)
                    c = !c;

                j = i++;
            }

            return c;
        }
    }
}