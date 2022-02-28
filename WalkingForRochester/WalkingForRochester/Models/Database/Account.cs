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
    public class Account
    {
        [JsonProperty("Id")] public int Id { get; set; }
        [JsonProperty("FirstName")] public string FirstName { get; set; }
        [JsonProperty("MidName")] public string MidName { get; set; }
        [JsonProperty("LastName")] public string LastName { get; set; }
        [JsonProperty("Email")] public string Email { get; set; }
        [JsonProperty("Password")] public string Password { get; set; }
        [JsonProperty("DateOfBirth")] public string DateOfBirth { get; set; }
        [JsonProperty("Address")] public string Address { get; set; }
        [JsonProperty("PhoneNumber")] public string PhoneNumber { get; set; }
        [JsonProperty("Active")] public int Active { get; set; }
        [JsonProperty("LoginLog")] public string LoginLog { get; set; }
        [JsonProperty("Permission")] public int Permission { get; set; }
        [JsonProperty("ImgUrl")] public string ImgUrl { get; set; }
        [JsonProperty("Nickname")] public string Nickname { get; set; }
    }
}