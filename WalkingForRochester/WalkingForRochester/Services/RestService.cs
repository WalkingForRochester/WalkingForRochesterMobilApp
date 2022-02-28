using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WalkingForRochester.Controls;
using WalkingForRochester.Maps;
using WalkingForRochester.Models;
using WalkingForRochester.Models.Database;
using WalkingForRochester.Models.Service;

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

namespace WalkingForRochester.Services
{
    public class RestService
    {
        // Global Url
        private const string Url = "https://walkingforrochester.com/php/";

        // Part of Url
        private const string SearchAccountPhp = "account.php";
        private const string InsertAccountPhp = "insert_account.php";
        private const string ModifyAccountPhp = "modify_account.php";
        private const string GmailInfoPhp = "gmail_info.php";
        private const string NewsFeedPhp = "news_feed.php";
        private const string LeaderboardPhp = "leaderboard.php";
        private const string UserProfilePhp = "user_profile.php";
        private const string PublishArticlePhp = "publish_article.php";
        private const string UploadImagePhp = "upload_image.php";
        private const string WalkWorkPhp = "walk_work.php";

        private const string JsonContent = "application/json";

        private static HttpClient _client;

        public RestService()
        {
            _client = new HttpClient();
        }

        public class Get
        {
            // Get Gmail Verification information
            public static async Task<GmailModel> GetGmailInfo()
            {
                try
                {
                    _client = new HttpClient();
                    var uri = new Uri(Url + GmailInfoPhp);
                    var response = await _client.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var connect = response.Content.ReadAsStringAsync().Result;
                        return JsonConvert.DeserializeObject<GmailModel>(connect);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("\tERROR {0}", e.Message);
                }

                return null;
            }

            // Get User Infomation
            public static async Task<List<Account>> GetAccount()
            {
                try
                {
                    _client = new HttpClient();
                    var uri = new Uri(Url + SearchAccountPhp);
                    var response = await _client.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                        return JsonConvert.DeserializeObject<List<Account>>(
                            response.Content.ReadAsStringAsync().Result);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("\tERROR {0}", e.Message);
                }

                return null;
            }

            // Get news feed list
            public static async Task<List<NewsFeed>> GetNewsFeed()
            {
                try
                {
                    _client = new HttpClient();
                    var uri = new Uri(Url + NewsFeedPhp);
                    var response = await _client.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = response.Content.ReadAsStringAsync().Result;
                        return JsonConvert.DeserializeObject<List<NewsFeed>>(
                            content);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("\tERROR {0}", e.Message);
                    throw;
                }

                return null;
            }

            // Get leaderboard list
            public static async Task<List<Leaderboard>> GetLeaderboardList()
            {
                try
                {
                    _client = new HttpClient();
                    var uri = new Uri(Url + LeaderboardPhp);
                    var response = await _client.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = response.Content.ReadAsStringAsync().Result;
                        return JsonConvert.DeserializeObject<List<Leaderboard>>(content);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("\tERROR {0}", e.Message);
                    throw;
                }

                return null;
            }

            // Get user profile information
            public static async Task<List<UserProfile>> GetUserProfile(UserProfileModel u)
            {
                try
                {
                    _client = new HttpClient();
                    var uri = new Uri(Url + UserProfilePhp);
                    var postAccountId = JsonConvert.SerializeObject(u);
                    var con = new StringContent(postAccountId, Encoding.UTF8, "application/json");
                    var response = await _client.PostAsync(uri, con);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = response.Content.ReadAsStringAsync().Result;
                        return JsonConvert.DeserializeObject<List<UserProfile>>(content);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("\tERROR {0}", e.Message);
                    throw;
                }

                return null;
            }
        }

        // update class
        public class Update
        {
            // update(change) user password
            public static async Task<bool> ResetPassword(ResetPasswordModel account)
            {
                try
                {
                    _client = new HttpClient();
                    var uri = new Uri(Url + ModifyAccountPhp);
                    var json = JsonConvert.SerializeObject(account);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _client.PutAsync(uri, content);
                    return response.IsSuccessStatusCode;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("\tERROR {0}", e.Message);
                }

                return false;
            }

            // Edit Profile then upload them
            public static async Task<bool> UploadProfile(EditProfileModel account, Stream image = null)
            {
                try
                {
                    _client = new HttpClient();

                    // check image exists
                    var uri = new Uri(Url + SearchAccountPhp);
                    var response = await _client.GetAsync(uri);
                    var time = DateTime.Today;

                    if (image != null)
                        account.Image = await Tools.UploadImage("Profile", account.UserId.ToString(), image,
                            time.ToShortDateString());

                    // update data
                    _client = new HttpClient();
                    uri = new Uri(Url + ModifyAccountPhp);
                    var json = JsonConvert.SerializeObject(account);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    response = await _client.PutAsync(uri, content);
                    return response.IsSuccessStatusCode;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("\tERROR {0}", e.Message);
                }

                return false;
            }
        }

        // insert class
        public class Insert
        {
            // register account
            public static async Task<bool> RegisterAccount(RegisterModel account, bool isNew = false)
            {
                try
                {
                    HttpResponseMessage response;
                    _client = new HttpClient();
                    var uri = new Uri(Url + InsertAccountPhp);
                    var json = JsonConvert.SerializeObject(account);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    if (isNew)
                        response = await _client.PostAsync(uri, content);
                    else
                        return false;

                    return response.IsSuccessStatusCode;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("\tERROR {0}", e.Message);
                }

                return false;
            }

            public static async Task<bool> SubmitWalkingWork(WalkModel wm, Stream img)
            {
                try
                {
                    var a = wm;
                    _client = new HttpClient();
                    var uri = new Uri(Url + WalkWorkPhp);
                    var time = DateTime.Today;
                    wm.PickImage = await Tools.UploadImage("Walking_PickImage", wm.UserId.ToString(), img,
                        time.ToShortDateString());
                    var json = JsonConvert.SerializeObject(wm);
                    var content = new StringContent(json, Encoding.UTF8, JsonContent);
                    var response = await _client.PostAsync(uri, content);
                    return response.IsSuccessStatusCode;
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Errpr: {e.Message}");
                    throw;
                }
            }

            // publish article to news feed
            public static async Task<bool> PublishArticle(ArticleModel am, Stream image)
            {
                try
                {
                    _client = new HttpClient();
                    var uri = new Uri(Url + PublishArticlePhp);
                    var time = DateTime.Today;
                    am.ImageName = await Tools.UploadImage("NewsFeed", am.Title, image, time.ToShortDateString());
                    var json = JsonConvert.SerializeObject(am);
                    var conJson = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _client.PostAsync(uri, conJson);
                    var result = response.Content.ReadAsStringAsync().Result;
                    Debug.WriteLine(result);
                    return true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Errpr: {e.Message}");
                    throw;
                }
            }
        }

        // delete class
        public class Delete
        {
        }

        // tool class
        private class Tools
        {
            // Upload the picture to server and get picture's name
            public static async Task<string> UploadImage(string category, string str, Stream image, string time = null)
            {
                _client = new HttpClient();
                var uri = new Uri(Url + UploadImagePhp);
                var form = new MultipartFormDataContent();
                var content = new StreamContent(image);

                string fileName;
                fileName = time != null
                    ? $"IMG_{category.ToUpper()}_{DateTime.Parse(time):MM_dd_yyyy}_{ToolKit.Encrypt32Bit(str)}"
                    : $"IMG_{category.ToUpper()}_{ToolKit.Encrypt32Bit(str)}";

                content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "file",
                    FileName = fileName
                };

                form.Add(content);
                var response = await _client.PostAsync(uri, form);
                var result = response.Content.ReadAsStringAsync().Result;
                Debug.WriteLine(result);
                return fileName;
            }
        }

        /// <summary>
        ///     Getting maps Geo file
        /// </summary>
        public class Resources
        {
            public static async Task<MapGeo> GetMapData()
            {
                _client = new HttpClient();
                var uri = new Uri("https://walkingforrochester.com/resources/map.json");
                var response = _client.GetAsync(uri).Result;

                if (!response.IsSuccessStatusCode) return null;

                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<MapGeo>(content);
            }
        }
    }
}