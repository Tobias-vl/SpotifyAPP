using System.Text.Json.Serialization;

namespace Spotify_backend.Models
{
    public class Track_Atr
    {
        [JsonPropertyName("id")]
        public string Track_id { get; set; }
        [JsonPropertyName("name")]
        public string Track_name { get; set; }
        
    }

    public class TrackItem
    {
        [JsonPropertyName("items")]
        public List<Teack> Track_item { get; set; }
    }

    public class Teack
    {
        [JsonPropertyName("track")]
        public Track_Atr track { get; set; }
    }

}
