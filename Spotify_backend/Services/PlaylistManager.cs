using Spotify_backend.Models;

namespace Spotify_backend.Services
{
    public class PlaylistManager
    {
        public readonly Dictionary<string, Playlist> _ListOfPlaylist = new();
    

        public void AddPlaylist(Playlist playlist)
        {
            _ListOfPlaylist.Add(playlist.Name, playlist);
        }
    }
}
