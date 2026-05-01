using System.Text.Json.Serialization;

public class CurrenttrackItem
{
    [JsonPropertyName("item")]
    public CurrentTrack Item { get; set; }

    [JsonPropertyName("is_playing")]
    public bool IsPlaying { get; set; }
}

public class CurrentTrack
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("artists")]
    public List<CurrentArtist> Artists { get; set; }

    [JsonPropertyName("duration_ms")]
    public int DurationMs { get; set; }
}

public class CurrentArtist
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}