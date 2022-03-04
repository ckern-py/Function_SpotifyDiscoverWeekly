using System.Collections.Generic;
using Newtonsoft.Json;

namespace SpotifyTypes.MetaData
{
    public class UpdateURI
    {
        [JsonProperty("uris")]
        public List<string> SpotifyUpdateUri { get; set; }
    }
}
