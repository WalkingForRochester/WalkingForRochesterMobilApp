using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using FFImageLoading;
using FFImageLoading.Config;
using FFImageLoading.Forms.Platform;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XF.Material.Droid;
using Platform = Xamarin.Essentials.Platform;

namespace WalkingForRochester.Droid
{
    [Activity(Label = "Walking For Rochester", Icon = "@mipmap/ROCicon", Theme = "@style/splashscreen",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                               ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.Window?.RequestFeature(WindowFeatures.ActionBar);
            base.SetTheme(Resource.Style.MainTheme);
            base.OnCreate(savedInstanceState);
            Forms.Init(this, savedInstanceState);
            FormsMaterial.Init(this, savedInstanceState);
            Platform.Init(this, savedInstanceState);
            Material.Init(this, savedInstanceState);

            var config = new Configuration {ExecuteCallbacksOnUIThread = false};
            ImageService.Instance.Initialize(config);
            CachedImageRenderer.Init(true);

            if (VersionTracking.IsFirstLaunchEver) SecureStorage.RemoveAll();

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            [GeneratedEnum] Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}