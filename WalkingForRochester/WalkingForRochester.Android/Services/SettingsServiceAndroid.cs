using Android;
using Android.App;
using Android.Content;
using Android.Locations;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using WalkingForRochester.Droid.Services;
using WalkingForRochester.Services;
using Xamarin.Forms;
using AlertDialog = AndroidX.AppCompat.App.AlertDialog;

[assembly: Xamarin.Forms.Dependency(typeof(SettingsServiceAndroid))]

namespace WalkingForRochester.Droid.Services
{
    public class SettingsServiceAndroid : ISettingsService
    {
        public void OpenSettings()
        {
#pragma warning disable 618
            Forms.Context.StartActivity(new Intent(Android.Provider.Settings.ActionLocationSourceSettings));
#pragma warning restore 618
        }
    }
}