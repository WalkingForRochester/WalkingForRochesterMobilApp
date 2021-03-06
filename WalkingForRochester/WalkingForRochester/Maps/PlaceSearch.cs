using System.Collections.Generic;
using Newtonsoft.Json;

namespace WalkingForRochester.Maps
{
    public class MatchedSubstring
    {
        [JsonProperty("length")] public int Length { get; set; }

        [JsonProperty("offset")] public int Offset { get; set; }
    }

    public class MainTextMatchedSubstring
    {
        [JsonProperty("length")] public int Length { get; set; }

        [JsonProperty("offset")] public int Offset { get; set; }
    }

    public class StructuredFormatting
    {
        [JsonProperty("main_text")] public string MainText { get; set; }

        [JsonProperty("main_text_matched_substrings")]
        public List<MainTextMatchedSubstring> MainTextMatchedSubstrings { get; set; }

        [JsonProperty("secondary_text")] public string SecondaryText { get; set; }
    }

    public class Term
    {
        [JsonProperty("offset")] public int Offset { get; set; }

        [JsonProperty("value")] public string Value { get; set; }
    }

    public class Prediction
    {
        [JsonProperty("description")] public string Description { get; set; }

        [JsonProperty("matched_substrings")] public List<MatchedSubstring> MatchedSubstrings { get; set; }

        [JsonProperty("place_id")] public string PlaceId { get; set; }

        [JsonProperty("reference")] public string Reference { get; set; }

        [JsonProperty("structured_formatting")]
        public StructuredFormatting StructuredFormatting { get; set; }

        [JsonProperty("terms")] public List<Term> Terms { get; set; }

        [JsonProperty("types")] public List<string> Types { get; set; }
    }

    public class PlaceSearch
    {
        [JsonProperty("predictions")] public List<Prediction> Predictions { get; set; }

        [JsonProperty("status")] public string Status { get; set; }
    }
}