using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using WalkingForRochester.Models;
using WalkingForRochester.Models.Database;
using WalkingForRochester.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using static WalkingForRochester.Converters.WeekOfDate.ConverterOfWeek;

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
    public class LeaderboardViewModel : BaseViewModel
    {
        public LeaderboardViewModel()
        {
            // Get User ID
            userId = JsonConvert.DeserializeObject<Account>(SecureStorage.GetAsync("CurrentUser").Result).Id;

            // init lists
            LeaderboardList = GetList();

            // calc collection / distances / duration
            LeaderboardListAllFilter = GetFilter();

            // Order by Desc with Distances
            Lists = LeaderboardListAllFilter;

            OwnSelf = LeaderboardListAllFilter.Where(x => x.AccountId == userId).ToList();
        }

        private List<LeaderboardModel> GetList()
        {
            try
            {
                var list = Task.Run(async () => await RestService.Get.GetLeaderboardList())
                    .Result
                    .Select(l => new LeaderboardModel
                    {
                        Id = l.Id,
                        AccountId = l.AccountId,
                        Collection = l.Collection,
                        Nickname = l.Nickname,
                        ImgUrl = new Uri(l.Img),
                        Date = l.Date,
                        Distances = l.Distance,
                        Duration = l.Duration
                    })
                    .ToList();
                return list;
            }
            catch (Exception e)
            {
                Debug.WriteLine("\tERROR {0}", e.Message);
                throw;
            }
        }

        private List<LeaderboardModel> GetFilter()
        {
            var list = LeaderboardList
                .AsEnumerable()
                .OrderByDescending(i => i.Date)
                .GroupBy(l => l.AccountId)
                .Select(g => new LeaderboardModel
                {
                    AccountId = g.Key,
                    Nickname = g.Select(i => i.Nickname).FirstOrDefault(),
                    Collection = g.Sum(x => x.Collection),
                    ImgUrl = g.Select(i => i.ImgUrl).FirstOrDefault(),
                    Date = g.Select(i => i.Date).FirstOrDefault(),
                    Distances = g.Sum(x => x.Distances),
                    Duration = g.Sum(x => x.Duration)
                })
                .OrderByDescending(i => i.Collection)
                .ToList();

            for (var i = 1; i <= list.Count; i++) list[i - 1].Id = i;

            return list;
        }

        private List<LeaderboardModel> GetCollectionWithDateTimeFilter(int d = -1)
        {
            var list = LeaderboardList
                .AsEnumerable()
                .OrderByDescending(i => i.Date)
                .ToList();

            // 0 = Day, 1 = Week, 2 = Month, 3 = Year
            var funcLists = new Func<List<LeaderboardModel>>(() =>
            {
                return d switch
                {
                    0 => list.AsEnumerable().Where(i => i.Date.Day == DateTime.Today.Day).ToList(),
                    1 => list.AsEnumerable()
                        .Where(i =>
                            i.Date >= ISO8601FirstDateOfWeek(DateTime.Today.Year,
                                ISO8601GetWeekOfYear(DateTime.Today)) &&
                            i.Date <= ISO8601EndDateOfWeek(DateTime.Today.Year, ISO8601GetWeekOfYear(DateTime.Today)))
                        .ToList(),
                    2 => list.AsEnumerable().Where(i => i.Date.Month == DateTime.Today.Month).ToList(),
                    3 => list.AsEnumerable().Where(i => i.Date.Year == DateTime.Today.Year).ToList(),
                    _ => list.ToList()
                };
            });

            var result = funcLists()
                .AsEnumerable()
                .OrderByDescending(i => i.Date)
                .GroupBy(l => l.AccountId)
                .Select(g => new LeaderboardModel
                {
                    AccountId = g.Key,
                    Nickname = g.Select(i => i.Nickname).FirstOrDefault(),
                    Collection = g.Sum(x => x.Collection),
                    ImgUrl = g.Select(i => i.ImgUrl).FirstOrDefault(),
                    Date = g.Select(i => i.Date).FirstOrDefault(),
                    Distances = g.Sum(x => x.Distances),
                    Duration = g.Sum(x => x.Duration),
                    CollectionVisible = true,
                    DistancesVisible = false,
                    DurationVisible = false
                })
                .OrderByDescending(i => i.Collection)
                .ToList();

            for (var i = 1; i <= result.Count; i++) result[i - 1].Id = i;

            OwnSelf = result.Where(x => x.AccountId == userId).ToList();

            return result;
        }

        private List<LeaderboardModel> GetDistanceWithDateTimeFilter(int d = -1)
        {
            var list = LeaderboardList
                .AsEnumerable()
                .OrderByDescending(i => i.Date)
                .ToList();

            // 0 = Day, 1 = Week, 2 = Month, 3 = Year
            var funcLists = new Func<List<LeaderboardModel>>(() =>
            {
                return d switch
                {
                    0 => list.AsEnumerable().Where(i => i.Date.Day == DateTime.Today.Day).ToList(),
                    1 => list.AsEnumerable()
                        .Where(i =>
                            i.Date >= ISO8601FirstDateOfWeek(DateTime.Today.Year,
                                ISO8601GetWeekOfYear(DateTime.Today)) &&
                            i.Date <= ISO8601EndDateOfWeek(DateTime.Today.Year, ISO8601GetWeekOfYear(DateTime.Today)))
                        .ToList(),
                    2 => list.AsEnumerable().Where(i => i.Date.Month == DateTime.Today.Month).ToList(),
                    3 => list.AsEnumerable().Where(i => i.Date.Year == DateTime.Today.Year).ToList(),
                    _ => list.ToList()
                };
            });

            var result = funcLists()
                .AsEnumerable()
                .OrderByDescending(i => i.Date)
                .GroupBy(l => l.AccountId)
                .Select(g => new LeaderboardModel
                {
                    AccountId = g.Key,
                    Nickname = g.Select(i => i.Nickname).FirstOrDefault(),
                    Collection = g.Sum(x => x.Collection),
                    ImgUrl = g.Select(i => i.ImgUrl).FirstOrDefault(),
                    Date = g.Select(i => i.Date).FirstOrDefault(),
                    Distances = g.Sum(x => x.Distances),
                    Duration = g.Sum(x => x.Duration),
                    CollectionVisible = false,
                    DistancesVisible = true,
                    DurationVisible = false
                })
                .OrderByDescending(i => i.Distances)
                .ToList();

            for (var i = 1; i <= result.Count; i++) result[i - 1].Id = i;

            OwnSelf = result.Where(x => x.AccountId == userId).ToList();

            return result;
        }

        private List<LeaderboardModel> GetDurationWithDateTimeFilter(int d = -1)
        {
            var list = LeaderboardList
                .AsEnumerable()
                .OrderByDescending(i => i.Date)
                .ToList();

            // 0 = Day, 1 = Week, 2 = Month, 3 = Year
            var funcLists = new Func<List<LeaderboardModel>>(() =>
            {
                return d switch
                {
                    0 => list.AsEnumerable().Where(i => i.Date.Day == DateTime.Today.Day).ToList(),
                    1 => list.AsEnumerable()
                        .Where(i =>
                            i.Date >= ISO8601FirstDateOfWeek(DateTime.Today.Year,
                                ISO8601GetWeekOfYear(DateTime.Today)) &&
                            i.Date <= ISO8601EndDateOfWeek(DateTime.Today.Year, ISO8601GetWeekOfYear(DateTime.Today)))
                        .ToList(),
                    2 => list.AsEnumerable().Where(i => i.Date.Month == DateTime.Today.Month).ToList(),
                    3 => list.AsEnumerable().Where(i => i.Date.Year == DateTime.Today.Year).ToList(),
                    _ => list.ToList()
                };
            });

            var result = funcLists()
                .AsEnumerable()
                .OrderByDescending(i => i.Date)
                .GroupBy(l => l.AccountId)
                .Select(g => new LeaderboardModel
                {
                    AccountId = g.Key,
                    Nickname = g.Select(i => i.Nickname).FirstOrDefault(),
                    Collection = g.Sum(x => x.Collection),
                    ImgUrl = g.Select(i => i.ImgUrl).FirstOrDefault(),
                    Date = g.Select(i => i.Date).FirstOrDefault(),
                    Distances = g.Sum(x => x.Distances),
                    Duration = g.Sum(x => x.Duration),
                    CollectionVisible = false,
                    DistancesVisible = false,
                    DurationVisible = true
                })
                .OrderByDescending(i => i.Duration)
                .ToList();

            for (var i = 1; i <= result.Count; i++)
            {
                result[i - 1].Id = i;
                result[i - 1].DurationFormat = $"{TimeSpan.FromMilliseconds(result[i - 1].Duration):g}";
            }

            OwnSelf = result.Where(x => x.AccountId == userId).ToList();
            return result;
        }

        private List<LeaderboardModel> GetDateTimeFilter(int d = -1)
        {
            var list = LeaderboardList
                .AsEnumerable()
                .OrderByDescending(i => i.Date)
                .ToList();

            // 0 = Day, 1 = Week, 2 = Month, 3 = Year
            var funcLists = new Func<List<LeaderboardModel>>(() =>
            {
                return d switch
                {
                    0 => list.AsEnumerable().Where(i => i.Date.Day == DateTime.Today.Day).ToList(),
                    1 => list.AsEnumerable()
                        .Where(i =>
                            i.Date >= ISO8601FirstDateOfWeek(DateTime.Today.Year,
                                ISO8601GetWeekOfYear(DateTime.Today)) &&
                            i.Date <= ISO8601EndDateOfWeek(DateTime.Today.Year, ISO8601GetWeekOfYear(DateTime.Today)))
                        .ToList(),
                    2 => list.AsEnumerable().Where(i => i.Date.Month == DateTime.Today.Month).ToList(),
                    3 => list.AsEnumerable().Where(i => i.Date.Year == DateTime.Today.Year).ToList(),
                    _ => list.ToList()
                };
            });

            var result = funcLists()
                .AsEnumerable()
                .OrderByDescending(i => i.Date)
                .GroupBy(l => l.AccountId)
                .Select(g => new LeaderboardModel
                {
                    AccountId = g.Key,
                    Nickname = g.Select(i => i.Nickname).FirstOrDefault(),
                    Collection = g.Sum(x => x.Collection),
                    ImgUrl = g.Select(i => i.ImgUrl).FirstOrDefault(),
                    Date = g.Select(i => i.Date).FirstOrDefault(),
                    Distances = g.Sum(x => x.Distances),
                    Duration = g.Sum(x => x.Duration)
                })
                .OrderByDescending(i => i.Collection)
                .ToList();

            for (var i = 1; i <= result.Count; i++) result[i - 1].Id = i;

            return result;
        }

        #region Props and Fields

        private readonly int userId;

        private List<LeaderboardModel> LeaderboardList { get; }
        private List<LeaderboardModel> LeaderboardListAllFilter { get; }

        private List<LeaderboardModel> lists;

        public List<LeaderboardModel> Lists
        {
            get => lists;
            set => SetProperty(ref lists, value);
        }

        private string eventPicker;

        private List<LeaderboardModel> ownSelf;

        public List<LeaderboardModel> OwnSelf
        {
            get => ownSelf;
            set => SetProperty(ref ownSelf, value);
        }

        public string EventPicker
        {
            get => eventPicker;
            set => SetProperty(ref eventPicker, value);
        }

        private string timePicker;

        public string TimePicker
        {
            get => timePicker;
            set => SetProperty(ref timePicker, value);
        }

        #endregion

        #region Commands and Events

        private ICommand eventPickerCommand;

        public ICommand EventPickerCommand => eventPickerCommand ??= new Command(() =>
        {
            if (!string.IsNullOrEmpty(TimePicker))
                Lists = TimePicker switch
                {
                    "Day" => EventPicker switch
                    {
                        "Distance" => GetDistanceWithDateTimeFilter(0),
                        "Duration" => GetDurationWithDateTimeFilter(0),
                        _ => GetCollectionWithDateTimeFilter(0)
                    },
                    "Week" => EventPicker switch
                    {
                        "Distance" => GetDistanceWithDateTimeFilter(1),
                        "Duration" => GetDurationWithDateTimeFilter(1),
                        _ => GetCollectionWithDateTimeFilter(1)
                    },
                    "Month" => EventPicker switch
                    {
                        "Distance" => GetDistanceWithDateTimeFilter(2),
                        "Duration" => GetDurationWithDateTimeFilter(2),
                        _ => GetCollectionWithDateTimeFilter(2)
                    },
                    "Year" => EventPicker switch
                    {
                        "Distance" => GetDistanceWithDateTimeFilter(3),
                        "Duration" => GetDurationWithDateTimeFilter(3),
                        _ => GetCollectionWithDateTimeFilter(3)
                    },
                    _ => GetFilter()
                };
            else
                Lists = EventPicker switch
                {
                    "Distance" => GetDistanceWithDateTimeFilter(),
                    "Duration" => GetDurationWithDateTimeFilter(),
                    _ => GetCollectionWithDateTimeFilter()
                };
        });


        private ICommand timePickerCommand;

        public ICommand TimePickerCommand => timePickerCommand ??= new Command(() =>
        {
            if (!string.IsNullOrEmpty(EventPicker))
                Lists = EventPicker switch
                {
                    "Collection" => TimePicker switch
                    {
                        "Day" => GetCollectionWithDateTimeFilter(0),
                        "Week" => GetCollectionWithDateTimeFilter(1),
                        "Month" => GetCollectionWithDateTimeFilter(2),
                        "Year" => GetCollectionWithDateTimeFilter(3),
                        _ => GetCollectionWithDateTimeFilter()
                    },
                    "Distance" => TimePicker switch
                    {
                        "Day" => GetDistanceWithDateTimeFilter(0),
                        "Week" => GetDistanceWithDateTimeFilter(1),
                        "Month" => GetDistanceWithDateTimeFilter(2),
                        "Year" => GetDistanceWithDateTimeFilter(3),
                        _ => GetDistanceWithDateTimeFilter()
                    },
                    "Duration" => TimePicker switch
                    {
                        "Day" => GetDurationWithDateTimeFilter(0),
                        "Week" => GetDurationWithDateTimeFilter(1),
                        "Month" => GetDurationWithDateTimeFilter(2),
                        "Year" => GetDurationWithDateTimeFilter(3),
                        _ => GetDurationWithDateTimeFilter()
                    },
                    _ => Lists
                };
            else
                Lists = TimePicker switch
                {
                    "Day" => GetDateTimeFilter(0),
                    "Week" => GetDateTimeFilter(1),
                    "Month" => GetDateTimeFilter(2),
                    "Year" => GetDateTimeFilter(3),
                    _ => GetDateTimeFilter()
                };
        });

        #endregion
    }
}