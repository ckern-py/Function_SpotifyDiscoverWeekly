using Newtonsoft.Json;

namespace SpotifyTypes.MetaData
{
    public class PlaylistTracks
    {
        [JsonProperty("href")]
        public string TracksHref { get; set; }

        [JsonProperty("total")]
        public int TracksTotal { get; set; }
    }
}
