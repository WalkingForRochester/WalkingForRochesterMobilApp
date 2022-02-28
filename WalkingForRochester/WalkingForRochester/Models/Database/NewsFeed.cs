using Newtonsoft.Json;

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

namespace WalkingForRochester.Models.Database
{
    public class NewsFeed
    {
        [JsonProperty("Id")] public int Id { get; set; }
        [JsonProperty("Title")] public string Title { get; set; }
        [JsonProperty("Author")] public string Author { get; set; }
        [JsonProperty("Content")] public string Content { get; set; }
        [JsonProperty("ImgUrl")] public string ImgUrl { get; set; }
        [JsonProperty("Publish")] public string Publish { get; set; }
    }
}