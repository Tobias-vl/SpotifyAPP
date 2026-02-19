using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Spotify_backend.Models
{


    public class PlaylistItems
    {
        [JsonPropertyName("href")]
        public string Href { get; set; }

        [JsonPropertyName("items")]
        public List<PlaylistTrackItem> Items { get; set; }

        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        [JsonPropertyName("next")]
        public string Next { get; set; }

        [JsonPropertyName("offset")]
        public int Offset { get; set; }

        [JsonPropertyName("previous")]
        public string Previous { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class PlaylistTrackItem
    {
        [JsonPropertyName("added_at")]
        public string AddedAt { get; set; }

        [JsonPropertyName("added_by")]
        public AddedByUser AddedBy { get; set; }

        [JsonPropertyName("is_local")]
        public bool IsLocal { get; set; }

        [JsonPropertyName("track")]
        public Track Track { get; set; }
    }

    public class AddedByUser
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class Track
    {
        [JsonPropertyName("album")]
        public Album Album { get; set; }

        [JsonPropertyName("artists")]
        public List<Artist> Artists { get; set; }

        [JsonPropertyName("duration_ms")]
        public int DurationMs { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("preview_url")]
        public string PreviewUrl { get; set; }
    }

    public class Album
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("images")]
        public List<Image> Images { get; set; }
    }

    public class Artist
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class Image
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }
    }

}
