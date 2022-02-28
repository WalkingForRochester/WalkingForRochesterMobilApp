using Android.App;
using Android.Content;
using Android.OS;
using Firebase.Messaging;

namespace WalkingForRochester.Droid.Services
{
    [Service]
    [IntentFilter(new[] {"com.google.firebase.MESSAGING_EVENT"})]
    public class FirebaseService : FirebaseMessagingService
    {
        private const string ApplicationTitle = "Walking For Rochester";
        private const string ChannelId = "com.walkingforrochester.walkingforrochester.android";

        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);

            SendLocalNotification();
        }

        private void SendLocalNotification()
        {
            const NotificationImportance importance = NotificationImportance.Low;

            var chan = new NotificationChannel(ChannelId, "WoR", importance);
            chan.EnableVibration(true);

            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            intent.PutExtra("Message", "");

            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            var notificationBuilder =
                new Notification.Builder(this, ChannelId)
                    .SetContentTitle(ApplicationTitle)
                    .SetSmallIcon(Resource.Mipmap.ROCicon)
                    // TODO: Notification Messages
                    .SetContentText("TODO")
                    .SetAutoCancel(true)
                    .SetContentIntent(pendingIntent);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                notificationBuilder.SetChannelId(ChannelId);

            var notificationManager = NotificationManager.FromContext(this);
            notificationManager?.CreateNotificationChannel(chan);
            notificationManager?.Notify(0, notificationBuilder.Build());
        }
    }
}