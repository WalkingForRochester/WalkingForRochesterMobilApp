using System;
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
    public class Leaderboard
    {
        [JsonProperty("Id")] public int Id { get; set; }
        [JsonProperty("AccountId")] public int AccountId { get; set; }
        [JsonProperty("Nickname")] public string Nickname { get; set; }
        [JsonProperty("ImgUrl")] public string Img { get; set; }
        [JsonProperty("Collection")] public int Collection { get; set; }
        [JsonProperty("Date")] public DateTime Date { get; set; }
        [JsonProperty("Distance")] public double Distance { get; set; }
        [JsonProperty("Duration")] public double Duration { get; set; }
    }
}