using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Plugin.Toast;
using WalkingForRochester.Controls;
using WalkingForRochester.Dependent;
using WalkingForRochester.Models;
using WalkingForRochester.Services;
using WalkingForRochester.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

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
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            PermissionRequest();
            PropertyChanged += (_, __) => ((Command) LoginCommand).ChangeCanExecute();
        }

        #region Permission Method

        private async void PermissionRequest()
        {
            try
            {
                if (!VersionTracking.IsFirstLaunchEver) return;

                var flag = await MaterialDialog.Instance.ConfirmAsync(
                    "Location: we need to gather your location in the background as you walk for your safety. We use the location data for our server. It will remain private. \n" +
                    "Camera: we need to access your camera for you to provide pictures of your volunteer work. \n" +
                    "Pictures provided may be used for news blogs content within this app but will remain private outside of the app.",
                    "Walking For Rochester need access to:",
                    "I agree",
                    "No");

                if (flag == true)
                {
                    switch (Device.RuntimePlatform)
                    {
                        case Device.Android:
                            var locationAlwaysPermission = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();
                            if (locationAlwaysPermission != PermissionStatus.Granted)
                                locationAlwaysPermission = await Permissions.RequestAsync<Permissions.LocationAlways>();
                            if (locationAlwaysPermission != PermissionStatus.Granted) return;
                            break;
                        case Device.iOS:
                            await PermissionDependency.LocationPermission();
                            break;
                    }
                }
                else
                {
                    await MaterialDialog.Instance.AlertAsync(
                        "Sorry! Since you deny to share your data, please log your walk and email your log to us.",
                        "Sorry");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        #endregion

        #region Commands

        private ICommand loginCommand;
        private ICommand registerCommand;
        private ICommand resetCommand;

        public ICommand LoginCommand => loginCommand ??= new Command(async () =>
            {
                LoginVisible = false;
                LoginActivity = true;

                try
                {
                    // Account Modal
                    var login = new LoginModel
                    {
                        Email = Email,
                        Password = Password
                    };

                    // Found account from database
                    var accounts = await RestService.Get.GetAccount();
                    accounts = accounts
                        .Where(
                            i =>
                                i.Email == login.Email &&
                                i.Password == ToolKit.Encrypt32Bit(login.Password))
                        .ToList();

                    if (accounts.Count > 0)
                    {
                        var account = accounts[0];
                        if (account.Active > 0)
                        {
                            await SecureStorage.SetAsync("CurrentUser", JsonConvert.SerializeObject(account));
                            await SecureStorage.SetAsync("isLogged", "1");

                            Application.Current.MainPage = new AppShell();
                            await Shell.Current.GoToAsync("//walk");
                            CrossToastPopUp.Current.ShowToastSuccess("Login Successfully");
                        }
                        else
                        {
                            LoginVisible = true;
                            LoginActivity = false;
                            SecureStorage.RemoveAll();
                            await Application.Current.MainPage.DisplayAlert("Login failed",
                                "Your account is invalid, please wait for the administrator's verification.",
                                "Ok");
                        }
                    }
                    else
                    {
                        LoginVisible = true;
                        LoginActivity = false;
                        await Application.Current.MainPage.DisplayAlert("Login failed",
                            "This account does not exist, please check if your email address and password are correct.",
                            "Ok");
                    }
                }
                catch (Exception e)
                {
                    LoginVisible = true;
                    LoginActivity = false;
                    Debug.WriteLine($"Error: {e.Message}");
                }
            }, () =>
                !string.IsNullOrEmpty(Email) &&
                !string.IsNullOrEmpty(Password)
        );

        public ICommand RegisterCommand => registerCommand ??=
            new Command(() => Application.Current.MainPage = new RegisterPage());

        public ICommand ResetCommand => resetCommand ??=
            new Command(() => Application.Current.MainPage = new ResetPasswordPage());

        #endregion

        #region Props

        private string email;
        private string password;

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        #endregion

        #region Control Props

        private bool loginActivity;
        private bool loginVisible = true;

        public bool LoginVisible
        {
            get => loginVisible;
            set => SetProperty(ref loginVisible, value);
        }

        public bool LoginActivity
        {
            get => loginActivity;
            set => SetProperty(ref loginActivity, value);
        }

        #endregion
    }
}