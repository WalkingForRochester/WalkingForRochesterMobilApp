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

namespace WalkingForRochester.Models.Service
{
    public class GmailModel
    {
        [JsonProperty("SenderMailAddress")] public string SenderMailAddress { get; set; }
        [JsonProperty("CredentialUsername")] public string CredentialUsername { get; set; }
        [JsonProperty("CredentialPassword")] public string CredentialPassword { get; set; }
        [JsonProperty("Subject")] public string Subject { get; set; }
    }
}