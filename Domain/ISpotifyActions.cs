using SpotifyTypes.MetaData;

namespace Domain
{
    public interface ISpotifyActions
    {
        MultiTemplate<Playlist> GetAllPlaylists();
        MultiTemplate<PlaylistItems> GetOnePlaylistTracks(string playListUID);
        void ReplacePlaylist(string playlistID, MultiTemplate<PlaylistItems> playlistToReplace);
    }
}
