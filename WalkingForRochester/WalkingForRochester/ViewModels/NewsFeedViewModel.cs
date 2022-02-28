using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
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
    public class NewsFeedViewModel : BaseViewModel
    {
        private List<NewsFeedModel> lists;
        private List<NewsFeedModel> listsFilter;
        private List<NewsFeedModel> listsUnFilter;

        private ICommand searchCommand;
        private string searchText;

        public NewsFeedViewModel()
        {
            var roles = JsonConvert.DeserializeObject<Account>(SecureStorage.GetAsync("CurrentUser").Result).Permission;

            Role = roles == 1;

            Mask = !Role;

            GetData();
        }

        public string SearchText
        {
            get => searchText;
            set => SetProperty(ref searchText, value);
        }

        public ICommand SearchCommand => searchCommand ??= new Command(() =>
        {
            if (string.IsNullOrWhiteSpace(SearchText))
                Lists = listsUnFilter;
            else
                Lists = GetSearchResults(SearchText);
        });

        public List<NewsFeedModel> Lists
        {
            get => lists;
            set => SetProperty(ref lists, value);
        }

        public List<NewsFeedModel> GetSearchResults(string queryString)
        {
            var normalizedQuery = queryString?.ToLower() ?? "";
            return listsFilter
                .Where(
                    x =>
                        x.Title.ToLower().Contains(normalizedQuery) ||
                        x.Publish.Month.ToString().Contains(normalizedQuery) ||
                        x.Publish.Year.ToString().Contains(normalizedQuery)
                )
                .ToList();
        }

        private async void GetData()
        {
            // IsBusy = true;
            Lists = new List<NewsFeedModel>
            {
                new NewsFeedModel()
                {
                    Title = "*************************",
                    Author = "Loading",
                    IsBusy = true
                },
                new NewsFeedModel()
                {
                    Title = "*************************",
                    Author = "Loading",
                    IsBusy = true
                },
                new NewsFeedModel()
                {
                    Title = "*************************",
                    Author = "Loading",
                    IsBusy = true
                }
            };

            IsBusy = true;
            await Task.Delay(5000);
            IsBusy = false;

            Lists = await ReadingNewFeedData();
            listsFilter = Lists;
            listsUnFilter = Lists;
        }

        private static async Task<List<NewsFeedModel>> ReadingNewFeedData()
        {
            try
            {
                var list = await RestService.Get.GetNewsFeed();
                if (list == null) return null;

                var checkList = list.Select(feed => new NewsFeedModel
                    {
                        Id = feed.Id,
                        Title = feed.Title,
                        Author = feed.Author,
                        Content = feed.Content,
                        ImgUrl = new Uri(feed.ImgUrl),
                        Publish = DateTime.Parse(feed.Publish)
                    })
                    .OrderByDescending(i => i.Publish)
                    .ThenByDescending(i => i.Id)
                    .ToList();

                return checkList;
            }
            catch (Exception e)
            {
                Debug.WriteLine("\tERROR {0}", e.Message);
                return null;
            }
        }

        #region Props

        private int id;
        private string title;
        private string author;
        private string content;
        private Uri imgUrl;
        private DateTime publish;
        private bool role;
        private bool mask = true;

        public bool Mask
        {
            get => mask;
            set => SetProperty(ref mask, value);
        }

        public bool Role
        {
            get => role;
            set => SetProperty(ref role, value);
        }

        public int Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public string Author
        {
            get => author;
            set => SetProperty(ref author, value);
        }

        public string Content
        {
            get => content;
            set => SetProperty(ref content, value);
        }

        public Uri ImgUrl
        {
            get => imgUrl;
            set => SetProperty(ref imgUrl, value);
        }

        public DateTime Publish
        {
            get => publish;
            set => SetProperty(ref publish, value);
        }

        private string dateSearchHandler;

        public string DateSearchHandler
        {
            get => dateSearchHandler;
            set => SetProperty(ref dateSearchHandler, value);
        }

        #endregion

        #region Command

        private ICommand publishNews;

        public ICommand PublishNews => publishNews ??= new Command(async () =>
        {
            await Shell.Current.GoToAsync(nameof(ArticlePage));
        });

        private ICommand refreshCommand;

        public ICommand RefreshCommand => refreshCommand ??= new Command(() =>
        {
            IsRefreshing = true;
            if (Lists != null)
                Lists.Clear();
            GetData();
            IsRefreshing = false;
        });

        #endregion
    }
}