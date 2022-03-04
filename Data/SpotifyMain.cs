using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using SpotifyTypes.MetaData;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class SpotifyMain : ISpotifyMain
    {
        private ISpotifyActions _spotifyActions;
        private ILogger _logger;

        public SpotifyMain(ILogger log)
        {
            _logger = log;
            _spotifyActions = new SpotifyActions(log);
        }

        public SpotifyMain(ILogger log, ISpotifyActions spotifyActions)
        {
            _logger = log;
            _spotifyActions = spotifyActions;
        }

        public void BackupPlaylist()
        {
            SpotifyMainLog("Start");
            string discWeeklyURI = string.Empty;
            string discLastWeekURI = string.Empty;

            SpotifyMainLog("Getting all Playlists");
            MultiTemplate<Playlist> allFoundPLists = _spotifyActions.GetAllPlaylists();

            SpotifyMainLog("Parsing names");
            foreach (Playlist playlist in allFoundPLists.TemplateItems)
            {
                if (playlist.Name == "Discover Weekly")
                {
                    discWeeklyURI = playlist.ID;
                }
                if (playlist.Name == "Discover Last Weekly")
                {
                    discLastWeekURI = playlist.ID;
                }
            }

            SpotifyMainLog("One Playlist tracks");
            MultiTemplate<PlaylistItems> diskWeeklyCurrTracks = _spotifyActions.GetOnePlaylistTracks(discWeeklyURI);

            SpotifyMainLog("Replace Playlist tracks");
            _spotifyActions.ReplacePlaylist(discLastWeekURI, diskWeeklyCurrTracks);

            SpotifyMainLog("Return");
        }

        private void SpotifyMainLog(string logString)
        {
            _logger.LogInformation($"BackupPlaylist - {logString} - {DateTime.Now}");
        }
    }
}
