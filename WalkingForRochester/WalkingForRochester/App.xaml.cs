using System;
using System.Diagnostics;
using WalkingForRochester.ViewModels;
using WalkingForRochester.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms;

[assembly: ExportFont("MaterialIcons-Regular.ttf", Alias = "CustomMaterialFont")]

namespace WalkingForRochester
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // plugins init here
            Material.Init(this);
        }

        protected override async void OnStart()
        {
            try
            {
                if (VersionTracking.IsFirstLaunchEver)
                {
                    SecureStorage.RemoveAll();
                    MainPage = new IntroPage();
                }
                else
                {
                    var isLogged = await SecureStorage.GetAsync("isLogged");
                    MainPage = isLogged == "1" ? (Page)new AppShell() : new LoginPage();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }


        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}