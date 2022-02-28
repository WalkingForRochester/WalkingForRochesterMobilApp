using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalkingForRochester.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WalkingForRochester.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
            BindingContext = new RegisterViewModel();

            var appTheme = AppInfo.RequestedTheme;
            if (appTheme == AppTheme.Dark)
            {
                // DOBPicker.TextColor = Color.White;
                DOBPicker.BackgroundColor = Color.White;
            }
        }

        private async void PhoneQuestion_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Phone Number", "Need contact information, will remain private", "OK");
        }

        private async void AddressQuestion_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Address", "Need address information, will remain private", "OK");
        }

        private async void DOBQuestion_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Date of Birth", "Must be 18 years or older, information will remain private", "OK");
        }
    }
}