using Android.Content;
using Android.Support.V7.Widget;
using WalkingForRochester;
using WalkingForRochester.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
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
/// customer renderer for Android toolbar appearance
/// </summary>

[assembly: ExportRenderer(typeof(AppShell), typeof(ShellCustomRenderer))]

namespace WalkingForRochester.Droid
{
    public class ShellCustomRenderer : ShellRenderer
    {
        public ShellCustomRenderer(Context context) : base(context)
        {
        }

        protected override IShellToolbarAppearanceTracker CreateToolbarAppearanceTracker()
        {
            return new ToolbarAppearanceTracker();
        }
    }

    public class ToolbarAppearanceTracker : IShellToolbarAppearanceTracker
    {
        public void Dispose()
        {
        }

        public void SetAppearance(AndroidX.AppCompat.Widget.Toolbar toolbar, IShellToolbarTracker toolbarTracker,
            ShellAppearance appearance)
        {
            // shell nav bar background color
            toolbar.SetBackgroundColor(Color.FromHex("#4a3787").ToAndroid());

            // check shell navigation bar has back button
            if (!toolbarTracker.CanNavigateBack)
                toolbar.SetNavigationIcon(Resource.Mipmap.navBtn_shadow);
        }

        public void ResetAppearance(AndroidX.AppCompat.Widget.Toolbar toolbar, IShellToolbarTracker toolbarTracker)
        {
            // shell nav bar background color
            toolbar.SetBackgroundColor(Color.FromHex("#4a3787").ToAndroid());
            
            // check shell navigation bar has back button
            if (!toolbarTracker.CanNavigateBack)
                toolbar.SetNavigationIcon(Resource.Mipmap.navBtn_shadow);
        }
    }
}