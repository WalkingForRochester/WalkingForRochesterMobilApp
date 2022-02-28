using System.Collections.Generic;
using Newtonsoft.Json;

namespace WalkingForRochester.Maps
{
    public class Candidate
    {
        [JsonProperty("formatted_address")] public string FormattedAddress { get; set; }

        [JsonProperty("icon")] public string Icon { get; set; }

        [JsonProperty("name")] public string Name { get; set; }
    }

    public class Place
    {
        [JsonProperty("candidates")] public List<Candidate> Candidates { get; set; }

        [JsonProperty("status")] public string Status { get; set; }
    }
}