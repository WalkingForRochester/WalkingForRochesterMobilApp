#nullable enable
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using ILocationListener = Android.Locations.ILocationListener;

namespace WalkingForRochester.Droid.Services
{
    [Service]
    public class DataSource : Service, ILocationListener
    {
        private const int ServiceRunningNotificationId = 10000;

        public override IBinder? OnBind(Intent? intent)
        {
            return null;
        }

        private LocationManager? lm;
        private double latitude;
        private double longitude;
        private double accuracy;

        public void OnLocationChanged(Location? location)
        {
            latitude = location!.Latitude;
            longitude = location.Longitude;
            accuracy = location.Accuracy;
        }

        public void OnProviderDisabled(string? provider)
        {
            Toast.MakeText(ApplicationContext, "Active GPS", ToastLength.Long)?.Show();
        }

        public void OnProviderEnabled(string? provider)
        {
            lm?.RequestLocationUpdates(LocationManager.GpsProvider, 10000, 10f, this);
        }

        public void OnStatusChanged(string? provider, [GeneratedEnum] Availability status, Bundle? extras)
        {
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent? intent, [GeneratedEnum] StartCommandFlags flags,
            int startId)
        {
            // Notification Listener
            var notification = new NotificationHelper().ReturnNotification();
            StartForeground(ServiceRunningNotificationId, notification);

            // Location Listener
            lm = (LocationManager) this.GetSystemService(Context.LocationService)!;
            lm?.RequestLocationUpdates(LocationManager.GpsProvider, 1000, 10f, this);
            lm?.RequestLocationUpdates(LocationManager.NetworkProvider, 1000, 1000f, this);

            return StartCommandResult.Sticky;
        }
    }
}