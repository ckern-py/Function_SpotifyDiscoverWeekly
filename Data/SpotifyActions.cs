using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Domain;
using Microsoft.Extensions.Logging;
using SpotifyTypes.MetaData;
using Newtonsoft.Json;

namespace Data
{
    public class SpotifyActions : ISpotifyActions
    {
        private ILogger _logger;
        private HttpClient _actionsClient;
        private Token _currToken;
        private IAuthorization _authorization;

        public SpotifyActions(ILogger log)
        {
            _logger = log;
            _actionsClient = new HttpClient();
            _authorization = new Authorization(log);
        }

        public MultiTemplate<Playlist> GetAllPlaylists()
        {
            ActionsLog("GetAllPlaylists", "Start");
            _currToken = _authorization.GetToken();

            _actionsClient.DefaultRequestHeaders.Clear();
            _actionsClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_currToken.AccessToken}");

            ActionsLog("GetAllPlaylists", "Send");
            HttpResponseMessage allUserPlaylists = _actionsClient.GetAsync("https://api.spotify.com/v1/me/playlists").GetAwaiter().GetResult();

            if (!allUserPlaylists.IsSuccessStatusCode)
            {
                throw new ApplicationException($"Error getting all Spotify playlists: {allUserPlaylists.ReasonPhrase}");
            }

            string returnedPlaylists = allUserPlaylists.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            ActionsLog("GetAllPlaylists", "Received");
            MultiTemplate<Playlist> allPlaylistsFound = JsonConvert.DeserializeObject<MultiTemplate<Playlist>>(returnedPlaylists);

            ActionsLog("GetAllPlaylists", "Return");
            return allPlaylistsFound;
        }

        public MultiTemplate<PlaylistItems> GetOnePlaylistTracks(string playListUID)
        {
            ActionsLog("GetOnePlaylistTracks", $"Start {playListUID}");
            _currToken = _authorization.GetToken();

            _actionsClient.DefaultRequestHeaders.Clear();
            _actionsClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_currToken.AccessToken}");

            ActionsLog("GetOnePlaylistTracks", $"Send {playListUID}");
            HttpResponseMessage onePlaylistResponse = _actionsClient.GetAsync($"https://api.spotify.com/v1/playlists/{playListUID}/tracks").GetAwaiter().GetResult();
            
            if (!onePlaylistResponse.IsSuccessStatusCode)
            {
                throw new ApplicationException($"Error getting tracks for one playlist, {playListUID} : {onePlaylistResponse.ReasonPhrase}");
            }

            string returnedPlaylistTracks = onePlaylistResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            ActionsLog("GetOnePlaylistTracks", $"Recieved {playListUID}");
            MultiTemplate<PlaylistItems> allPlaylistTracks = JsonConvert.DeserializeObject<MultiTemplate<PlaylistItems>>(returnedPlaylistTracks);

            ActionsLog("GetOnePlaylistTracks", $"Return {playListUID}");
            return allPlaylistTracks;
        }

        public void ReplacePlaylist(string playlistID, MultiTemplate<PlaylistItems> playlistToReplace)
        {
            ActionsLog("GetOnePlaylistTracks", $"Start {playlistID}");
            _currToken = _authorization.GetToken();

            List<string> replaceList = new List<string>();

            foreach (PlaylistItems plistItems in playlistToReplace.TemplateItems)
            {
                replaceList.Add(plistItems.Track.URI);
            }

            ActionsLog("GetOnePlaylistTracks", $"Extraction {playlistID}");

            UpdateURI update = new UpdateURI
            {
                SpotifyUpdateUri = replaceList
            };
                        
            string replaceContent = JsonConvert.SerializeObject(update);
            StringContent populateContent = new StringContent(replaceContent, Encoding.Default, "application/json");

            ActionsLog("GetOnePlaylistTracks", $"Content Created {playlistID}");
            _actionsClient.DefaultRequestHeaders.Clear();
            _actionsClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_currToken.AccessToken}");

            ActionsLog("GetOnePlaylistTracks", $"Send {playlistID}");
            HttpResponseMessage playlistARemoval = _actionsClient.PutAsync($"https://api.spotify.com/v1/playlists/{playlistID}/tracks", populateContent).GetAwaiter().GetResult();

            if (!playlistARemoval.IsSuccessStatusCode)
            {
                throw new ApplicationException($"Error replaceing tracks for one playlist, {playlistID} : {playlistARemoval.ReasonPhrase}");
            }

            ActionsLog("GetOnePlaylistTracks", $"Recieved {playlistID}");
        }

        private void ActionsLog(string method, string logString)
        {
            _logger.LogInformation($"{method} - {logString} - {DateTime.Now}");
        }
    }
}
