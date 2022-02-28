using System.Threading.Tasks;
using Plugin.Toast;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WalkingForRochester.Dependent
{
    public static class PermissionDependency
    {
        public static async Task<bool> CameraPermission()
        {
            if (Device.RuntimePlatform == Device.Android) return true;
            
            var cameraPermission = await Permissions.CheckStatusAsync<Permissions.Camera>();

            switch (cameraPermission)
            {
                case PermissionStatus.Granted:
                    await Permissions.RequestAsync<Permissions.Camera>();
                    return true;
                // case PermissionStatus.Denied:
                //     CrossToastPopUp.Current.ShowToastError("You have denied the camera permission. If you want to turn it on again, please go to the settings to modify it.");
                //     return false;
                // case PermissionStatus.Restricted:
                //     CrossToastPopUp.Current.ShowToastError("You have restricted the camera permission.");
                //     return true;
                // case PermissionStatus.Disabled:
                //     CrossToastPopUp.Current.ShowToastError("You have disabled the camera permission.");
                //     return false;
                case PermissionStatus.Unknown:
                    await Permissions.RequestAsync<Permissions.Camera>();
                    return true;
                default:
                    return true;
            }
        }

        public static async Task<bool> PhotoPermission()
        {
            if (Device.RuntimePlatform == Device.Android) return true;
            
            var photoPermission = await Permissions.CheckStatusAsync<Permissions.Photos>();
            switch (photoPermission)
            {
                case PermissionStatus.Granted:
                    await Permissions.RequestAsync<Permissions.Photos>();
                    return true;
                // case PermissionStatus.Denied:
                //     CrossToastPopUp.Current.ShowToastError("You have denied the photos permission. If you want to turn it on again, please go to the settings to modify it.");
                //     return false;
                // case PermissionStatus.Restricted:
                //     CrossToastPopUp.Current.ShowToastError("You have restricted the photos permission.");
                //     return true;
                // case PermissionStatus.Disabled:
                //     CrossToastPopUp.Current.ShowToastError("You have disabled the photos permission.");
                //     return false;
                case PermissionStatus.Unknown:
                    await Permissions.RequestAsync<Permissions.Photos>();
                    return true;
                default:
                    return true;
            }
        }

        public static async Task<bool> LocationPermission()
        {
            var locationPermission = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            
            switch (locationPermission)
            {
                case PermissionStatus.Granted:
                    await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    return true;
                // case PermissionStatus.Denied:
                //     CrossToastPopUp.Current.ShowToastError("You have denied the location permission. If you want to turn it on again, please go to the settings to modify it.");
                //     return false;
                // case PermissionStatus.Restricted:
                //     CrossToastPopUp.Current.ShowToastError("You have restricted the location permission.");
                //     return true;
                // case PermissionStatus.Disabled:
                //     CrossToastPopUp.Current.ShowToastError("You have disabled the location permission.");
                //     return false;
                // case PermissionStatus.Unknown:
                //     await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                //     return true;
                default:
                    return true;
            }
        }

        public static async Task<bool> AlwaysLocationPermission()
        {
            var locationAlwaysPermission = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();
            
            switch (locationAlwaysPermission)
            {
                case PermissionStatus.Granted:
                    await Permissions.RequestAsync<Permissions.LocationAlways>();
                    return true;
                // case PermissionStatus.Denied:
                //     CrossToastPopUp.Current.ShowToastError("You have denied the always location permission. If you want to turn it on again, please go to the settings to modify it.");
                //     return false;
                // case PermissionStatus.Restricted:
                //     CrossToastPopUp.Current.ShowToastError("You have restricted the always location permission.");
                //     return true;
                // case PermissionStatus.Disabled:
                //     CrossToastPopUp.Current.ShowToastError("You have disabled the always location permission.");
                //     return false;
                case PermissionStatus.Unknown:
                    await Permissions.RequestAsync<Permissions.LocationAlways>();
                    return true;
                default:
                    return true;
            }
        }
    }
}