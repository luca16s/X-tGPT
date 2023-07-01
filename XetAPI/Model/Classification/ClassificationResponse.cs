namespace XetAPI.Model.Classification
{
    using Newtonsoft.Json;

    public class ClassificationResponse
    {
        [JsonProperty("sequence")]
        public required string Question { get; set; }

        [JsonProperty("labels")]
        public required string[] Labels { get; set; }

        [JsonProperty("scores")]
        public required List<double> Scores { get; set; }
    }
}
