namespace XetAPI.Model.Embedding
{
    using Newtonsoft.Json;

    public class EmbeddingModel
    {
        [JsonProperty("model")]
        public required string Model { get; set; }

        [JsonProperty("texts")]
        public required List<string> Texts { get; set; }
    }
}
