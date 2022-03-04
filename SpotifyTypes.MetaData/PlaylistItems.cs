using System.Collections.Generic;
using Newtonsoft.Json;

namespace SpotifyTypes.MetaData
{
    public class PlaylistItems
    {
        [JsonProperty("added_at")]
        public string AddedAt { get; set; }

        [JsonProperty("added_by")]
        public Owner AddedBy { get; set; }

        [JsonProperty("is_local")]
        public bool IsLocal { get; set; }

        [JsonProperty("primary_color")]
        public string PrimaryColor { get; set; }

        [JsonProperty("track")]
        public FullTrack Track { get; set; }

        [JsonProperty("video_thumbnail")]
        public Dictionary<string, string> VideoThumbnail { get; set; }
    }
}
