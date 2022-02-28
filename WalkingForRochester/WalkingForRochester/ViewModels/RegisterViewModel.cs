using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows.Input;
using Newtonsoft.Json;
using Plugin.Toast;
using WalkingForRochester.Controls;
using WalkingForRochester.Maps;
using WalkingForRochester.Models;
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
    public class RegisterViewModel : BaseViewModel
    {
        public RegisterViewModel()
        {
            RegisterCommand = new Command(OnRegisterClicked, ValidateForm);
            PropertyChanged += (_, __) => RegisterCommand.ChangeCanExecute();
        }

        private bool ValidateForm()
        {
            #region Validate Email

            bool validateEmail = false;
            if (!string.IsNullOrEmpty(Email)) validateEmail = EmailError = !ToolKit.ValidateEmail(Email);

            #endregion

            #region Validate PhoneNumber

            bool validatePhoneNumber = false;
            if (!string.IsNullOrEmpty(PhoneNumber)) validatePhoneNumber = PhoneNumberError = PhoneNumber.Length < 10;

            #endregion

            #region Validate Date of Birth

            // Validate format Date of Birth
            bool validateDob = false;
            //if (ToolKit.ValidateDateOfBirth(DateOfBirth))
            //{
            // Calculate age and validate 
            if (!ToolKit.Over18Age(DateOfBirth))
            {
                if (DeviceInfo.Platform == DevicePlatform.Android)
                    CrossToastPopUp.Current.ShowToastError("Age must be over 18.");

                DateOfBirthError = true;
                DateOfBirthErrorText = "Invalidate, must be over 18 age";
                validateDob = true;
            }
            else
            {
                DateOfBirthError = false;
                DateOfBirthErrorText = "";
            }
            //}
            //else
            //{
            //    DateOfBirthError = true;
            //    DateOfBirthErrorText = "Invalidate, Not match format";
            //}

            #endregion

            #region Validate Password

            if (!string.IsNullOrEmpty(Password) && Password.Length < 6)
            {
                PasswordError = true;
                PasswordErrorText = "invalidate, password must be over 6.";
            }
            else
            {
                PasswordError = false;
                PasswordErrorText = "";
            }

            if (!string.IsNullOrEmpty(ConfirmPassword) && ConfirmPassword.Length < 6)
            {
                ConfirmPasswordError = true;
                ConfirmPasswordErrorText = "invalidate, password must be over 6.";
            }
            else
            {
                ConfirmPasswordError = false;
                ConfirmPasswordErrorText = "";
            }

            #endregion

            return !string.IsNullOrEmpty(FirstName) &&
                   !string.IsNullOrEmpty(LastName) &&
                   !string.IsNullOrEmpty(Email) &&
                   !validateEmail &&
                   !string.IsNullOrEmpty(PhoneNumber) &&
                   !validatePhoneNumber &&
                   !validateDob &&
                   !string.IsNullOrEmpty(Address) &&
                   !string.IsNullOrEmpty(Password) &&
                   !string.IsNullOrEmpty(ConfirmPassword) &&
                   Password == ConfirmPassword;
        }

        private async void OnRegisterClicked()
        {
            if (Password == confirmPassword)
                try
                {
                    var register = new RegisterModel
                    {
                        FirstName = FirstName,
                        LastName = LastName,
                        EmailAddress = Email,
                        Password = ToolKit.Encrypt32Bit(Password),
                        PhoneNumber = PhoneNumber,
                        Address = Address,
                        DateOfBirth = DateOfBirth
                    };

                    // Check email address exists
                    var account = await RestService.Get.GetAccount();
                    var b = account.Any(i => i.Email == register.EmailAddress);

                    if (await RestService.Insert.RegisterAccount(register, !b))
                    {
                        CrossToastPopUp.Current.ShowToastSuccess(
                            "Register success!");
                        Application.Current.MainPage = new LoginPage();
                    }
                    else
                    {
                        CrossToastPopUp.Current.ShowToastError("Sorry, This email address does exists.");
                    }
                }
                catch (Exception e)
                {
                    await Application.Current.MainPage.DisplayAlert("Error",
                        "An unknown error has occurred, please contact Walking for Rochester, Inc.", "Ok");
                    Debug.WriteLine("\tERROR {0}", e.Message);
                }
            else
                CrossToastPopUp.Current.ShowToastError("Sorry, Password mismatch.");
        }

        #region Commands

        public Command RegisterCommand { get; }

        private ICommand cancelCommand;
        private ICommand callSearchCommand;
        private ICommand searchCancelCommand;
        private ICommand searchCommand;
        private ICommand searchSelectedCommand;
        private ICommand dateOfBirthChangedCommand;

        public ICommand CancelCommand => cancelCommand
            ??= new Command(() => Application.Current.MainPage = new LoginPage());

        public ICommand CallSearchCommand => callSearchCommand
            ??= new Command(() => SearchVisible = true);

        public ICommand SearchCancelCommand => searchCancelCommand
            ??= new Command(() => SearchVisible = false);

        public ICommand SearchCommand => searchCommand
            ??= new Command<string>(async e =>
            {
                if (string.IsNullOrWhiteSpace(e))
                {
                    SearchResults = null;
                }
                else
                {
                    const string key = "AIzaSyB3XuxlSSBT_cdTLrNhYC0OLHgvoVZxsqw";
                    var url = $"https://maps.googleapis.com/maps/api/place/autocomplete/json?input={e}&key={key}";
                    using var wc = new WebClient();
                    var jsonStr = await wc.DownloadStringTaskAsync(url);
                    var results = JsonConvert.DeserializeObject<PlaceSearch>(jsonStr);
                    SearchResults = results.Predictions;
                }
            });

        public ICommand SearchSelectedCommand => searchSelectedCommand
            ??= new Command<Prediction>(obj =>
            {
                SearchVisible = false;
                Address = $"{obj.Description}";
            });

        public ICommand DateOfBirthChangedCommand => dateOfBirthChangedCommand
            ??= new Command<DateTime>((dateSelected) => { DateOfBirth = dateSelected.ToLocalTime(); });

        #endregion

        #region Fields

        private List<Prediction> searchResults;

        public List<Prediction> SearchResults
        {
            get => searchResults;
            set => SetProperty(ref searchResults, value);
        }

        #endregion

        #region Props

        private string firstName;
        private string lastName;
        private string email;
        private string phoneNumber;
        private string address;
        private DateTime dateOfBirth = DateTime.Today;
        private string password;
        private string confirmPassword;
        private bool emailError;
        private bool phoneNumberError;
        private bool passwordError, confirmPasswordError;
        private string passwordErrorText, confirmPasswordErrorText;
        private string dateOfBirthErrorText;
        private bool dateOfBirthError;

        public string FirstName
        {
            get => firstName;
            set => SetProperty(ref firstName, value);
        }

        public string LastName
        {
            get => lastName;
            set => SetProperty(ref lastName, value);
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

        public string Address
        {
            get => address;
            set => SetProperty(ref address, value);
        }

        public DateTime DateOfBirth
        {
            get => dateOfBirth;
            set => SetProperty(ref dateOfBirth, value.Date);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public string ConfirmPassword
        {
            get => confirmPassword;
            set => SetProperty(ref confirmPassword, value);
        }

        public string DateOfBirthErrorText
        {
            get => dateOfBirthErrorText;
            set => SetProperty(ref dateOfBirthErrorText, value);
        }

        public bool DateOfBirthError
        {
            get => dateOfBirthError;
            set => SetProperty(ref dateOfBirthError, value);
        }

        public bool EmailError
        {
            get => emailError;
            set => SetProperty(ref emailError, value);
        }

        public bool PhoneNumberError
        {
            get => phoneNumberError;
            set => SetProperty(ref phoneNumberError, value);
        }

        public bool PasswordError
        {
            get => passwordError;
            set => SetProperty(ref passwordError, value);
        }

        public bool ConfirmPasswordError
        {
            get => confirmPasswordError;
            set => SetProperty(ref confirmPasswordError, value);
        }

        public string PasswordErrorText
        {
            get => passwordErrorText;
            set => SetProperty(ref passwordErrorText, value);
        }

        public string ConfirmPasswordErrorText
        {
            get => confirmPasswordErrorText;
            set => SetProperty(ref confirmPasswordErrorText, value);
        }

        private bool searchVisible;

        public bool SearchVisible
        {
            get => searchVisible;
            set => SetProperty(ref searchVisible, value);
        }

        #endregion
    }
}