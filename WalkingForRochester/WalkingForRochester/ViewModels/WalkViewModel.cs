using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Toast;
using WalkingForRochester.Dependent;
using WalkingForRochester.Maps;
using WalkingForRochester.Models;
using WalkingForRochester.Models.Database;
using WalkingForRochester.Models.Service;
using WalkingForRochester.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Maps;
using Location = Xamarin.Essentials.Location;
using Map = Xamarin.Forms.Maps.Map;
using Position = Xamarin.Forms.Maps.Position;

/// <summary>
/// Authors: 
///         NTID MAD TEAM
///             Mangers:        Xiangyu Shi
///                             Michelle Olson
///                         
///             Programmer:     Xiangyu Shi, 
///                             Michelle Olson, 
///                             Zhencheng Chen,
///                             Harsh Mathur,
///                             Quoc Nhan,
///                             Chase Roth  
/// </summary>

namespace WalkingForRochester.ViewModels
{
    public class WalkViewModel : BaseViewModel
    {
        // get account's permission
        private int? role = JsonConvert.DeserializeObject<Account>(SecureStorage.GetAsync("CurrentUser").Result)
            ?.Permission;

        public WalkViewModel()
        {
            #region Commands

            StartCommand = new Command(OnStartClicked);
            SaveCommand = new Command(OnSaveClicked);
            PickImageCommand = new Command(OnMediaClicked);
            CloseCommand = new Command(OnCloseClicked);

            #endregion

            var geoLocation = GetCurrentLocation();

            if (geoLocation.Result != null)
            {
                // Positioning coordinates
                var position = new Position(geoLocation.Result.Latitude, geoLocation.Result.Longitude);
                var mapSpan = new MapSpan(position, .01, .01);

                // Show mapset
                Map = new Map(mapSpan);
            }

            // Map init
            Map.IsShowingUser = true;
            Map.MapElements.Add(SetPolygonWithLimitCounty());
            poly = new Polyline {StrokeColor = Color.Blue, StrokeWidth = 12};
            Map.MapElements.Add(poly);

            // check user with in Rochester/Monroe area
            AreaWithIn();

            // set test mode for test account
            IsTestVisible = role == 2 ? true : false;
        }

        #region Limit with in area

        private async void AreaWithIn()
        {
            var location = await GetCurrentLocation();
            var isIn = await LimitArea.AreaWithin(new Position(location.Latitude, location.Longitude));
            if (isIn) return;
            ActiveVisible = false;
            await Application.Current.MainPage.DisplayAlert("Sorry!", "You do not live within the Monroe county area.",
                "Ok");
        }

        #endregion

        #region Commands

        private async void OnStartClicked()
        {
            if (ActiveButton.ToLower() == "start")
            {
                // Ask user allow always location used
                if (Device.RuntimePlatform == Device.Android)
                    if (!await PermissionDependency.AlwaysLocationPermission())
                        return;

                stopwatch = new Stopwatch();
                stopwatch.Start();
                ActiveButton = "Done";
                await StartListening();
                if (Device.RuntimePlatform == Device.Android)
                    DependencyService.Get<IAndroidService>().StartService();
            }
            else
            {
                stopwatch.Stop();
                await StopListening();
                CalcDistances();
                AdditionalFrameVisible = true;
                ActiveVisible = false;
                if (Device.RuntimePlatform == Device.Android)
                    DependencyService.Get<IAndroidService>().StopService();
            }
        }

        private void CalcDistances()
        {
            var distance = 0.0;
            var polyData = poly.Geopath.ToList();
            for (var i = 1; i < polyData.Count; i++)
            {
                var l1 = polyData[i - 1];
                var l2 = polyData[i];
                distance += CalculateDistance(l1.Latitude, l1.Longitude, l2.Latitude, l2.Longitude);
            }

            Distances = Math.Round(distance, 2);
        }

        private async void OnSaveClicked()
        {
            IsRunning = true;
            IsPending = !IsRunning;
            if (string.IsNullOrEmpty(NoteText) && PickImageSource == null)
            {
                IsRunning = false;
                IsPending = !IsRunning;
                await Application.Current.MainPage.DisplayAlert("Sorry, cannot save your route.",
                    "Notes need to be filled out and Image must be picked before you can save your information.",
                    "Ok");
                return;
            }
            else if (string.IsNullOrEmpty(NoteText))
            {
                IsRunning = false;
                IsPending = !IsRunning;
                await Application.Current.MainPage.DisplayAlert("Sorry, cannot save your route.",
                    "Please fill out notes about your route.", "Ok");
                return;
            }
            else if (PickImageSource == null)
            {
                IsRunning = false;
                IsPending = !IsRunning;
                await Application.Current.MainPage.DisplayAlert("Sorry, cannot save your route.",
                    "Please either choose image to best describe your route to brag your walk.", "Ok");
                return;
            }

            var positionJson = JsonConvert.SerializeObject(Map.Pins.Select(i =>
                    new PositionModel
                    {
                        Latitude = i.Position.Latitude,
                        Longitude = i.Position.Longitude
                    })
                .ToList());

            var walkModel = new WalkModel
            {
                UserId = JsonConvert.DeserializeObject<Account>(SecureStorage.GetAsync("CurrentUser").Result).Id,
                Collection = Collection,
                Distances = Distances,
                NoteText = NoteText,
                PickDate = PickDate,
                PinData = positionJson,
                WalkingTime = stopwatch.Elapsed.TotalMilliseconds
            };

            var result = await RestService.Insert.SubmitWalkingWork(walkModel, photo.GetStream());
            if (result)
            {
                NoteText = string.Empty;
                PickDate = DateTime.Today;
                ScreenshotSource = null;
                PickImageSource = null;
                Distances = 0;
                Collection = 0;
                Map.Pins.Clear();
                poly.Geopath.Clear();
                ActiveButton = "Start";
                AdditionalFrameVisible = false;
                ActiveVisible = true;
                stopwatch.Reset();
                IsRunning = true;
                IsPending = !IsRunning;
                CrossToastPopUp.Current.ShowToastSuccess("Saved and Sent!");
            }
            else
            {
                CrossToastPopUp.Current.ShowToastError("Sorry! There have problem. Please feedback.");
                IsRunning = false;
                IsPending = !IsRunning;
            }
        }

        private async void OnMediaClicked()
        {
            // Ask user allow camera permission
            if (!await PermissionDependency.CameraPermission()) return;

            photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions());
            if (photo != null) PickImageSource = ImageSource.FromStream(() => photo.GetStream());
        }

        private void OnCloseClicked()
        {
            //to close the notes so can view the map
            AdditionalFrameVisible = false;
            ActiveVisible = true;
        }

        #endregion

        #region maps Features

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var theta = lon1 - lon2;
            var dist = Math.Sin(DegToRad(lat1)) * Math.Sin(DegToRad(lat2)) +
                       Math.Cos(DegToRad(lat1)) * Math.Cos(DegToRad(lat2)) * Math.Cos(DegToRad(theta));
            dist = Math.Acos(dist);
            dist = RadToDeg(dist);
            dist = dist * 60 * 1.1515;
            var distance = Distance.FromMiles(dist);

            return distance.Miles;
        }

        private double DegToRad(double deg)
        {
            return deg * Math.PI / 180.0;
        }

        private double RadToDeg(double rad)
        {
            return rad / Math.PI * 180.0;
        }

        private async Task<Location> GetCurrentLocation()
        {
            Location location = null;
            try
            {
                location = await Geolocation.GetLastKnownLocationAsync();
                if (location == null)
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.High,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
            }
            catch (Exception ex)
            {
                // Unable to get location
                Log.Warning("Error - Exception: ", ex.Message);
            }

            return location;
        }

        /// <summary>
        ///     drawing limit area
        /// </summary>
        /// <returns></returns>
        private Polygon SetPolygonWithLimitCounty()
        {
            var polygon = new Polygon
            {
                StrokeWidth = 2,
                StrokeColor = Color.Red,
                FillColor = Color.FromHex("#30F08080"),
                // Monroe Country area
                Geopath =
                {
                    new Position(43.26070608211925, -77.78594970703125),
                    new Position(43.18164775501797, -77.79968261718744),
                    new Position(43.08945063454079, -77.7996826171874),
                    new Position(43.03125492868639, -77.76123046875),
                    new Position(43.01669737169671, -77.68501281738276),
                    new Position(43.01067254527255, -77.5978088378907),
                    new Position(43.014689161895184, -77.5126647949218),
                    new Position(43.03125492868639, -77.43164062500006),
                    new Position(43.071395809535375, -77.3863220214844),
                    new Position(43.15710884095329, -77.3773956298828),
                    new Position(43.23269713756249, -77.3780822753906),
                    new Position(43.27020619076863, -77.41859436035156),
                    new Position(43.26720631662829, -77.431640625),
                    new Position(43.26270622820009, -77.44537353515625),
                    new Position(43.257205668363206, -77.4742126464844),
                    new Position(43.247703530924454, -77.508544921875),
                    new Position(43.23970058108125, -77.52227783203125),
                    new Position(43.2351984597904, -77.5408172607422),
                    new Position(43.24070100730572, -77.57171630859375),
                    new Position(43.256705592827764, -77.596435546875),
                    new Position(43.27020619076863, -77.62321472167963),
                    new Position(43.28020470342185, -77.6513671875),
                    new Position(43.29769814751322, -77.68226623535156),
                    new Position(43.30669281678247, -77.69805908203125),
                    new Position(43.30919109985686, -77.7104187011719),
                    new Position(43.31468696117798, -77.7227783203125),
                    new Position(43.31968276747214, -77.7117919921875),
                    new Position(43.32517767999296, -77.7159118652344),
                    new Position(43.33067209551502, -77.73170471191406),
                    new Position(43.33866308542215, -77.7639770507812),
                    new Position(43.26070608211925, -77.78594970703125)
                }
            };

            return polygon;
        }

        #endregion

        #region Listening Position

        public async Task StartListening()
        {
            // isTracking = true;
            if (CrossGeolocator.Current.IsListening) return;

            Plugin.Geolocator.Abstractions.Position p;

            if (Device.RuntimePlatform == Device.Android)
                p = await CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromSeconds(1));
            else
                p = await CrossGeolocator.Current.GetLastKnownLocationAsync();

            var pin = new Pin
            {
                Position = new Position(p.Latitude, p.Longitude),
                Type = PinType.Generic,
                Label = "Start"
            };

            Map.Pins.Add(pin);

            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(5), 10, true);
                    break;
                case Device.iOS:
                    await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(5), 10, true,
                        new ListenerSettings
                        {
                            ActivityType = ActivityType.Fitness,
                            DeferLocationUpdates = true,
                            DeferralDistanceMeters = 10,
                            DeferralTime = TimeSpan.FromSeconds(2),
                            PauseLocationUpdatesAutomatically = false
                        });
                    break;
            }

            CrossGeolocator.Current.PositionChanged += PositionChanged;
        }

        private async Task StopListening()
        {
            if (!CrossGeolocator.Current.IsListening)
                return;

            await CrossGeolocator.Current.StopListeningAsync();

            Plugin.Geolocator.Abstractions.Position p;

            if (Device.RuntimePlatform == Device.Android)
                p = await CrossGeolocator.Current.GetPositionAsync();
            else
                p = await CrossGeolocator.Current.GetLastKnownLocationAsync();

            var pin = new Pin
            {
                Position = new Position(p.Latitude, p.Longitude),
                Type = PinType.Generic,
                Label = "End"
            };
            Map.Pins.Add(pin);
            CrossGeolocator.Current.PositionChanged -= PositionChanged;
        }

        private void PositionChanged(object sender, PositionEventArgs e)
        {
            //If updating the UI, ensure you invoke on main thread
            var position = e.Position;
            poly.Geopath.Add(new Position(position.Latitude, position.Longitude));
        }

        #endregion

        #region Fields

        private Polyline poly;
        public Map Map { get; set; }
        private MediaFile photo;
        private Stopwatch stopwatch;

        #endregion

        #region Properties

        private string searchPlaceholder;
        private string activeButton = "Start";
        private bool activeVisible = true;
        private bool additionalFrameVisible;
        private DateTime pickDate = DateTime.Today;
        private ImageSource screenshotSource;
        private ImageSource pickImageSource = ImageSource.FromFile("AddImages.png");
        private string noteText;
        private double distances;

        public int Collection
        {
            get => collection;
            set => SetProperty(ref collection, value);
        }

        private int collection;


        public double Distances
        {
            get => distances;
            set => SetProperty(ref distances, value);
        }

        public string SearchPlaceholder
        {
            get => searchPlaceholder;
            set => SetProperty(ref searchPlaceholder, value);
        }

        public string ActiveButton
        {
            get => activeButton;
            set => SetProperty(ref activeButton, value);
        }

        public bool AdditionalFrameVisible
        {
            get => additionalFrameVisible;
            set => SetProperty(ref additionalFrameVisible, value);
        }

        public bool ActiveVisible
        {
            get => activeVisible;
            set => SetProperty(ref activeVisible, value);
        }

        public DateTime PickDate
        {
            get => pickDate;
            set => SetProperty(ref pickDate, value);
        }

        public ImageSource ScreenshotSource
        {
            get => screenshotSource;
            set => SetProperty(ref screenshotSource, value);
        }

        public ImageSource PickImageSource
        {
            get => pickImageSource;
            set => SetProperty(ref pickImageSource, value);
        }

        public string NoteText
        {
            get => noteText;
            set => SetProperty(ref noteText, value);
        }

        #endregion

        #region Command

        public Command StartCommand { get; }
        public Command SaveCommand { get; }
        public Command PickImageCommand { get; }
        public Command CloseCommand { get; }

        private ICommand gpsCommand;

        public ICommand GpsCommand => gpsCommand ??= new Command(async () =>
        {
            var p = await GetCurrentLocation();
            var mapSpan = new MapSpan(new Position(p.Latitude, p.Longitude), .01, .01);
            Map.MoveToRegion(mapSpan);
        });

        private bool isToggled;
        private bool isTestVisible;

        public bool IsToggled
        {
            get
            {
                if (role == 2)
                    ActiveVisible = isToggled;
                return isToggled;
            }
            set => SetProperty(ref isToggled, value);
        }

        public bool IsTestVisible
        {
            get => isTestVisible;
            set => SetProperty(ref isTestVisible, value);
        }

        #endregion
    }
}