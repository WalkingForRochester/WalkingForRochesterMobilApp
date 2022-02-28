using System.Diagnostics;
using AppTrackingTransparency;
using FFImageLoading.Forms.Platform;
using Foundation;
using Rg.Plugins.Popup;
using UIKit;
using Xamarin;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XF.Material.iOS;

namespace WalkingForRochester.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //f
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Init();
            LoadApplication(new App());

            FormsMaps.Init();
            Popup.Init();
            Material.Init();
            CachedImageRenderer.Init();
            FormsMaterial.Init();

            if (VersionTracking.IsFirstLaunchEver) SecureStorage.RemoveAll();

            // Add ATTracking Transparency follow Apple Inc.
            ATTrackingManager.RequestTrackingAuthorization((status) =>
            {
                if (status == ATTrackingManagerAuthorizationStatus.Authorized)
                {
                    Debug.WriteLine("Tracking Authorized");
                }
                else if (status == ATTrackingManagerAuthorizationStatus.Denied)
                {
                    Debug.WriteLine("Tracking Denied");
                }
            });

            
            return base.FinishedLaunching(app, options);
        }
        
        
    }
}