using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using WalkingForRochester.Models;
using WalkingForRochester.Models.Database;
using WalkingForRochester.Services;
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
    public class UserProfileViewModel : BaseViewModel
    {
        public UserProfileViewModel()
        {
            var user = GetUser().Result;
            AccountId = user.Id;
            Nickname = user.Nickname;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            ImageUri = user.ImgUrl;
            Distance = Math.Round(user.Distance, 2);
            TotalDistance = Math.Round(user.TotalDistance, 2);

            Duration = TimeSpan.FromMilliseconds(user.Duration).ToString("g");
            TotalDuration = TimeSpan.FromMilliseconds(user.TotalDuration).ToString("g");
        }

        private async Task<UserProfileModel> GetUser()
        {
            try
            {
                var userid = Convert.ToInt32(JsonConvert
                    .DeserializeObject<Account>(await SecureStorage.GetAsync("CurrentUser")).Id);
                var tempUser = new UserProfileModel {UserId = userid};
                var user = Task.Run(async () => await RestService.Get.GetUserProfile(tempUser)).Result[0];
                var a = new UserProfileModel
                {
                    Id = user.Id,
                    Nickname = user.Nickname,
                    ImgUrl = new Uri(user.ImgUrl),
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Distance = user.Distance == string.Empty ? 0 : Convert.ToDouble(user.Distance),
                    TotalDistance = user.TotalDistance == string.Empty ? 0 : Convert.ToDouble(user.TotalDistance),
                    Duration = user.Duration == string.Empty ? 0 : Convert.ToDouble(user.Duration),
                    TotalDuration = user.TotalDuration == string.Empty ? 0 : Convert.ToDouble(user.TotalDuration)
                };
                return a;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error: {e.Message}");
            }

            return null;
        }

        #region Commands

        private ICommand logoutCommand;
        private ICommand profileCommand;

        public ICommand LogoutCommand => logoutCommand ??= new Command(() =>
        {
            // Clear all Secure Storage data
            SecureStorage.RemoveAll();

            // Switch to Login Page
            Application.Current.MainPage = new LoginPage();
        });

        public ICommand ProfileCommand => profileCommand ??= new Command(async () =>
        {
            await Shell.Current.GoToAsync(nameof(EditPage));
        });

        #endregion

        #region Props

        private int accountId;
        private string nickname;
        private Uri imageUri;
        private string email;
        private string phoneNumber;
        private double distance;
        private double totalDistance;
        private string duration;
        private string totalDuration;

        public int AccountId
        {
            get => accountId;
            set => SetProperty(ref accountId, value);
        }

        public string Nickname
        {
            get => nickname;
            set => SetProperty(ref nickname, value);
        }

        public Uri ImageUri
        {
            get => imageUri;
            set => SetProperty(ref imageUri, value);
        }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string PhoneNumber
        {
            get => phoneNumber;
            set => SetProperty(ref phoneNumber, value);
        }

        public double Distance
        {
            get => distance;
            set => SetProperty(ref distance, value);
        }

        public double TotalDistance
        {
            get => totalDistance;
            set => SetProperty(ref totalDistance, value);
        }

        public string Duration
        {
            get => duration;
            set => SetProperty(ref duration, value);
        }

        public string TotalDuration
        {
            get => totalDuration;
            set => SetProperty(ref totalDuration, value);
        }

        #endregion
    }
}