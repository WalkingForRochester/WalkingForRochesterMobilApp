using System.Net;
using Newtonsoft.Json;
using WalkingForRochester.Maps;
using WalkingForRochester.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace WalkingForRochester.Controls
{
    public class MapSearchHandler : SearchHandler
    {
        private readonly string key = "AIzaSyB3XuxlSSBT_cdTLrNhYC0OLHgvoVZxsqw";

        protected override async void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);
            var url = "https://maps.googleapis.com/maps/api/place/findplacefromtext/json?input=";
            var fields = "&inputtype=textquery&fields=formatted_address,name,icon";

            if (string.IsNullOrWhiteSpace(newValue) || newValue.Length < 3)
                ItemsSource = null;
            else
            {
                using var wc = new WebClient();
                var jsonStr = await wc.DownloadStringTaskAsync(url + newValue + fields + "&key=" + key);
                var place = JsonConvert.DeserializeObject<Place>(jsonStr);
                ItemsSource = place.Candidates;
            }
        }

        protected override async void OnItemSelected(object item)
        {
            base.OnItemSelected(item);
            var place = (Candidate) item;
            Location loc;

            var url = "https://maps.googleapis.com/maps/api/geocode/json?fields=geometry/location&address=" +
                      place.FormattedAddress + "&key=" + key;
            using (var wc = new WebClient())
            {
                var jsonStr = await wc.DownloadStringTaskAsync(url);
                var coor = JsonConvert.DeserializeObject<LatLng>(jsonStr);
                loc = coor.Results[0].Geometry.Location;
            }

            // Positioning coordinates
            var position = new Position(loc.Lat, loc.Lng);
            var mapSpan = new MapSpan(position, .01, .01);
            var vm = (WalkViewModel) MapHelper.CurrentVM;

            vm.Map.MoveToRegion(mapSpan);
        }
    }
}