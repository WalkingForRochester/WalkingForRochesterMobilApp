using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using WalkingForRochester.Droid.Services;
using Xamarin.Forms;
using Application = Android.App.Application;

[assembly: Dependency(typeof(NotificationHelper))]

namespace WalkingForRochester.Droid.Services
{
    public class NotificationHelper : INotification
    {
        private static readonly string foregroundChannelId = "9001";
        
#pragma warning disable 618
        private static readonly Context context = Xamarin.Forms.Forms.Context;
#pragma warning restore 618

        public Notification ReturnNotification()
        {
            // Building intent
            var intent = new Intent(context, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.SingleTop);
            intent.PutExtra("Title", "Message");

            var pendingIntent = PendingIntent.GetActivity(context, 0, intent, PendingIntentFlags.UpdateCurrent);

            var notifBuilder = new NotificationCompat.Builder(context, foregroundChannelId)
                .SetContentTitle("Walking For Rochester Server")
                .SetContentText("Walking For Rochester Using Walking Server Tracker")
                .SetSmallIcon(Resource.Mipmap.NotificationIcon)
                .SetContentIntent(pendingIntent)
                .SetOngoing(true);

            // Building channel if API verion is 26 or above
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var notificationChannel =
                    new NotificationChannel(foregroundChannelId, "Title", NotificationImportance.High);
                notificationChannel.Importance = NotificationImportance.High;
                notificationChannel.EnableLights(true);
                notificationChannel.EnableVibration(true);
                notificationChannel.SetShowBadge(true);
            
                if (context.GetSystemService(Context.NotificationService) is NotificationManager notifManager)
                {
                    notifBuilder.SetChannelId(foregroundChannelId);
                    notifManager.CreateNotificationChannel(notificationChannel);
                }
            }

            return notifBuilder.Build();
        }

        PendingIntent BuildIntentToShowMainActivity()
        {
            var notificationIntent = new Intent(context, typeof(MainActivity));
            notificationIntent.SetAction("WalkingForRochester.action.MAIN_ACTIVITY");
            notificationIntent.SetFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTask);
            notificationIntent.PutExtra("has_service_been_started", true);

            var pendingIntent =
                PendingIntent.GetActivity(context, 0, notificationIntent, PendingIntentFlags.NoCreate);
            return pendingIntent;
        }
    }
}