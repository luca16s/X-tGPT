namespace XetAPI.Model.Classification
{
    using Newtonsoft.Json;

    public class ClassificationModel
    {
        [JsonProperty("model")]
        public required string Model { get; set; }

        [JsonProperty("pipeline")]
        public required string Pipeline { get; set; }

        [JsonProperty("question")]
        public required string Question { get; set; }

        [JsonProperty("labels")]
        public required string[] Labels { get; set; }
    }
}
