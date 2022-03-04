using Newtonsoft.Json;

namespace SpotifyTypes.MetaData
{
    public class Images
    {
        [JsonProperty("height")]
        public int? Height { get; set; }

        [JsonProperty("url")]
        public string ImageURL { get; set; }

        [JsonProperty("width")]
        public int? Width { get; set; }
    }
}
