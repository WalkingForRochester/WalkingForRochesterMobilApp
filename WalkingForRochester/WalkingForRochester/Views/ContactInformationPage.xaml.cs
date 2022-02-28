using System;
using WalkingForRochester.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WalkingForRochester.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactInformationPage : ContentPage
    {
        public ContactInformationPage()
        {
            InitializeComponent();

            BindingContext = new ContactViewModel();
        }
    }
}