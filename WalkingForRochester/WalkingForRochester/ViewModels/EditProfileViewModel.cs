using System;
using System.Linq;
using System.Windows.Input;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Toast;
using WalkingForRochester.Dependent;
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
    public class EditProfileViewModel : BaseViewModel
    {
        private string email;
        private ImageSource imgSource;
        private ICommand mediaCommand;
        private string nickname;
        private string phone;

        private MediaFile photo;
        private ICommand uploadCommand;

        private Account account;

        public EditProfileViewModel()
        {
            account = JsonConvert.DeserializeObject<Account>(SecureStorage.GetAsync("CurrentUser").Result);

            GetData();

            PropertyChanged += (_, __) => ((Command) UploadCommand).ChangeCanExecute();
        }

        public ImageSource ImgSource
        {
            get => imgSource;
            set => SetProperty(ref imgSource, value);
        }

        public string Nickname
        {
            get => nickname;
            set => SetProperty(ref nickname, value);
        }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string Phone
        {
            get => phone;
            set => SetProperty(ref phone, value);
        }

        public ICommand UploadCommand => uploadCommand ??= new Command(async () =>
            {
                try
                {
                    var model = new EditProfileModel
                    {
                        UserId = account.Id,
                        Nickname = Nickname,
                        EmailAddress = Email,
                        PhoneNumber = Phone,
                        Image = ImgSource.ToString().Substring(5)
                    };

                    var pending = false;

                    if (model.Image == account.ImgUrl)
                        pending = await RestService.Update.UploadProfile(model);
                    else
                        pending = await RestService.Update.UploadProfile(model, photo.GetStream());

                    if (pending)
                    {
                        var a = await RestService.Get.GetAccount();
                        var aL = a.Where(i => i.Id == account.Id).ToList();
                        var newAccount = aL[0];
                        SecureStorage.Remove("CurrentUser");
                        await SecureStorage.SetAsync("CurrentUser", JsonConvert.SerializeObject(newAccount));
                        await Shell.Current.GoToAsync("..");
                        CrossToastPopUp.Current.ShowToastSuccess(
                            "The update is successful, restart and open the application or log in again to take effect.");
                    }
                    else
                        CrossToastPopUp.Current.ShowToastError("Failed!");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            },
            () =>
                !string.IsNullOrEmpty(Nickname) &&
                !string.IsNullOrEmpty(Email) &&
                !string.IsNullOrEmpty(Phone) &&
                Phone.Length >= 10
        );

        public ICommand MediaCommand => mediaCommand ??= new Command(async () =>
        {
            // Ask user allow pick photo permission
            if (await PermissionDependency.PhotoPermission())
            {
                photo = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions {PhotoSize = PhotoSize.Medium});
                if (photo != null) ImgSource = ImageSource.FromStream(() => photo.GetStream());
            }
        });

        private void GetData()
        {
            Nickname = account.Nickname;
            Email = account.Email;
            Phone = account.PhoneNumber;
            ImgSource = ImageSource.FromUri(new Uri(account.ImgUrl));
        }
    }
}