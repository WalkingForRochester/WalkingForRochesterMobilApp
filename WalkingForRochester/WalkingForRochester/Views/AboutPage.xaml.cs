using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WalkingForRochester.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            Label1.Text =
                "When your neighborhood is littered and in need of trash removal, " +
                "you have the potential to transform it into something better. " +
                "With the help of Walking For Rochester, " +
                "we can help you and your neighbors take the initiative to clean up your streets and build a better community around you. " +
                "We're a non-profit, 100% volunteer-based organization striving to connect and bring people of all backgrounds together over something that affects us all. " +
                "Our volunteers are located throughout the Greater Rochester, NY, region, " +
                "and by cleaning up one neighborhood and street at a time, " +
                "we can inspire leaders and strengthen our city and region as a whole.";
            
            Label2.Text =
                "\"Being born and raised in Rochester, NY, " +
                "you really start to get a feel for your community. " +
                "Two things that always stuck out to me is that garbage is almost everywhere and not everyone feels connected to their neighborhood. " +
                "No matter how nice or poor the neighborhood, there just always seems to be litter on our streets and a disconnect with our neighbors. " +
                "Walking For Rochester aims to change not only our streets but also our hearts. " +
                "It takes more than people picking up garbage to make a difference. " +
                "We also need to regain a sense of community and belonging. " +
                "We are stronger together, and once we start to pick up litter and become leaders in our community, others will follow suit! " +
                "It starts with a piece of litter and then blossoms into a beautiful community that you and I are proud of. " +
                "Sign up today and see how the power of one person can make a big difference with the support of others!\"" +
                "\n\nFounder and President \nMatt Apple";
        }

        private async void XiangyuLinkedlnButton_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://www.linkedin.com/in/xiangyu-shi/", BrowserLaunchMode.SystemPreferred);
        }

        private async void ColtonLinkedlnButton_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://www.linkedin.com/in/colton-b/", BrowserLaunchMode.SystemPreferred);
        }

        private async void HarshLinkedlnButton_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://www.linkedin.com/in/harsh-m97/", BrowserLaunchMode.SystemPreferred);
        }

        private async void MichelleLinkedlnButton_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://www.linkedin.com/in/michelle-m-olson/", BrowserLaunchMode.SystemPreferred);
        }

        private async void ZhenchengLinkedlnButton_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://www.linkedin.com/in/zhencheng-chen/", BrowserLaunchMode.SystemPreferred);
        }

        private async void RothLinkedInButton_Clicked(object sender, EventArgs e)
        {
            //waiting on new link from Chase
            await Browser.OpenAsync("https://www.linkedin.com/in/chase-roth/", BrowserLaunchMode.SystemPreferred);
        }

        private async void QuocLinkedInButton_Clicked(object sender, EventArgs e)
        {
            //Update
            await Browser.OpenAsync("https://www.linkedin.com/in/quocpeace/", BrowserLaunchMode.SystemPreferred);
        }
    }
}