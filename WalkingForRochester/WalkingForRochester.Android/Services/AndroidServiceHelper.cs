using Android.Content;
using Android.OS;
using WalkingForRochester.Droid.Services;
using WalkingForRochester.Services;
using Xamarin.Forms;
using Application = Android.App.Application;

[assembly: Dependency(typeof(AndroidServiceHelper))]

namespace WalkingForRochester.Droid.Services
{
    internal class AndroidServiceHelper : IAndroidService
    {
        private readonly Context context = Application.Context;

        public void StartService()
        {
            var intent = new Intent(context, typeof(DataSource));

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                context.StartForegroundService(intent);
            else
                context.StartService(intent);
        }

        public void StopService()
        {
            var intent = new Intent(context, typeof(DataSource));
            context.StopService(intent);
        }
    }
}