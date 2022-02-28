using System;
using System.Diagnostics;
using System.Windows.Input;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Toast;
using WalkingForRochester.Dependent;
using WalkingForRochester.Models;
using WalkingForRochester.Services;
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
    public class ArticleViewModel : BaseViewModel
    {
        //fields

        private string articleTitle;
        private string articleAuthor;
        private DateTime articleDate = DateTime.Today;
        private string articleContent;
        private ImageSource imageSource = ImageSource.FromFile("AddImages.png");
        private MediaFile photo;
        private ICommand mediaCommand;
        private ICommand articleSaveCommand;
        private bool isSubmit = true;

        public bool IsSubmit { get => isSubmit; set => isSubmit = value; }

        // props

        public string ArticleTitle
        {
            get => articleTitle;
            set => SetProperty(ref articleTitle, value);
        }

        public DateTime ArticleDate
        {
            get => articleDate;
            set => SetProperty(ref articleDate, value);
        }

        public string ArticleAuthor
        {
            get => articleAuthor;
            set => SetProperty(ref articleAuthor, value);
        }

        public string ArticleContent
        {
            get => articleContent;
            set => SetProperty(ref articleContent, value);
        }

        public ImageSource ImageSource
        {
            get => imageSource;
            set => SetProperty(ref imageSource, value);
        }

        //ICommands methtods
        public ICommand MediaCommand => mediaCommand ??= new Command(async () =>
        {
            try
            {
                // Ask user allow pick photo permission
                if (!await PermissionDependency.PhotoPermission()) return;

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }

                photo = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Medium
                });

                if (photo != null) ImageSource = ImageSource.FromStream(() => photo.GetStream());
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error: MediaCommand ERROR, messages: {e.Message}");
                throw;
            }
        });

        public ICommand ArticleSaveCommand => articleSaveCommand ??= new Command(async () =>
            {
                try
                {
                    var newPublish = new ArticleModel
                    {
                        Title = ArticleTitle,
                        Date = ArticleDate.ToShortDateString(),
                        Author = ArticleAuthor,
                        Content = articleContent
                    };
                    var b = await RestService.Insert.PublishArticle(newPublish, photo.GetStream());
                    if (b)
                    {
                        await Shell.Current.GoToAsync("..");
                        CrossToastPopUp.Current.ShowToastSuccess("Published!");
                    }
                    else
                    {
                        CrossToastPopUp.Current.ShowToastError("Publish failed!");
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"ERROR: {e.Message}");
                }
            }, () =>
                !string.IsNullOrEmpty(ArticleTitle) &&
                !string.IsNullOrEmpty(ArticleAuthor) &&
                ImageSource != null &&
                !string.IsNullOrEmpty(ArticleContent)
        );

        public ArticleViewModel()
        {
            PropertyChanged += (sender, args) => ((Command)ArticleSaveCommand).ChangeCanExecute();
        }
    }
}