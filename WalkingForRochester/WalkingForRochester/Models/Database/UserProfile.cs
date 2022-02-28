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
    public class UserProfile
    {
        [JsonProperty("Id")] public int Id { get; set; }
        [JsonProperty("Nickname")] public string Nickname { get; set; }
        [JsonProperty("Email")] public string Email { get; set; }
        [JsonProperty("PhoneNumber")] public string PhoneNumber { get; set; }
        [JsonProperty("ImgUrl")] public string ImgUrl { get; set; }

        [JsonProperty("Distance")] public string Distance { get; set; }

        [JsonProperty("TotalDistance")] public string TotalDistance { get; set; }

        [JsonProperty("Duration")] public string Duration { get; set; }

        [JsonProperty("TotalDuration")] public string TotalDuration { get; set; }
    }
}