using System;
using System.Windows.Input;
using Newtonsoft.Json;
using WalkingForRochester.Models.Database;
using WalkingForRochester.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

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
/// </summary>

namespace WalkingForRochester.ViewModels
{
    public class AppShellViewModel : BaseViewModel
    {
        private Uri imgUrl = new Uri("https://walkingforrochester.com/images/default_img.png");
        private string nickname = "Guest";

        private ICommand userProfileCommand;

        public AppShellViewModel()
        {
            var account = JsonConvert.DeserializeObject<Account>(SecureStorage.GetAsync("CurrentUser").Result);
            Nickname = account.Nickname;
            ImgUrl = new Uri(account.ImgUrl);
        }

        public string Nickname
        {
            get => nickname;
            set => SetProperty(ref nickname, value);
        }

        public Uri ImgUrl
        {
            get => imgUrl;
            set => SetProperty(ref imgUrl, value);
        }

        public ICommand UserProfileCommand => userProfileCommand ??=
            new Command(async () => await Shell.Current.GoToAsync(nameof(UserProfilePage)));
    }
}