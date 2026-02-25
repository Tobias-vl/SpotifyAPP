using System.Text.Json.Serialization;

namespace Spotify_backend.Models
{

    public class PlaylistOwner
    {
        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }
    }
    public class Playlist
    {
        public string Id { get; set; }
        public string Name { get; set; }

        [JsonPropertyName("owner")]
        public PlaylistOwner Owner { get; set; }
    }

    public class Playlists
    {
        [JsonPropertyName("items")]
        public List<Playlist> Items { get; set; }
    }
}
