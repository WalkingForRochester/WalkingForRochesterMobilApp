using Plugin.Toast;
using System;
using System.Threading.Tasks;
using Plugin.Geolocator;
using WalkingForRochester.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace WalkingForRochester.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WalkPage : ContentPage
    {
        private WalkViewModel vm;

        public WalkPage()
        {
            InitializeComponent();

            vm = new WalkViewModel();
            BindingContext = vm;
            MapHelper.CurrentVM = vm;
        }
    }
}