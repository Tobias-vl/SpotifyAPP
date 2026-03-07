using System.Text.Json.Serialization;

namespace Spotify_backend.Models
{
    public class Device
    {
        [JsonPropertyName("devices")]
        public List<Deivce_att> device {  get; set; }

    }

    public class Deivce_att
    {
        [JsonPropertyName("id")]
        public string id { get; set; }
        public string name { get; set; }
        public bool is_active { get; set; }
        public bool is_restricted { get; set; }
    }
}
