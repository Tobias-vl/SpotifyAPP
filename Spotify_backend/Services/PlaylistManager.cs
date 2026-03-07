using Spotify_backend.Models;

namespace Spotify_backend.Services
{
    public class PlaylistManager
    {
        public readonly Dictionary<string, TrackItem> Playlist = new();
    

        public void AddPlaylist(TrackItem playlistitem)
        {
            Playlist.Add(playlistitem.Track_owner, playlistitem);
        }
    }
}
