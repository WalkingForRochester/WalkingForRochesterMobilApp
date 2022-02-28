using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using WalkingForRochester.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WalkingForRochester.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeaderboardPage : ContentPage
    {
        public LeaderboardPage()
        {
            InitializeComponent();
            
            PickerTimeColor.TextColor = Color.White;
            PickerEventColor.TextColor = Color.White;
        }
    }
}