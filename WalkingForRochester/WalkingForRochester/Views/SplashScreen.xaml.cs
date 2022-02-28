using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WalkingForRochester.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashScreen : ContentPage
    {
        public SplashScreen()
        {
            InitializeComponent();       
            
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            stackLayout.Opacity = 0;
            stackLayout.FadeTo(1, 3000);
        }
    }
}