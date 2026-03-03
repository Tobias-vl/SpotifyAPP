using System.Text.Json.Serialization;

public class CurrenttrackItem
{
    [JsonPropertyName("item")]
    public CurrentTrack Item { get; set; }
}

public class CurrentTrack
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("artists")]
    public List<CurrentArtist> Artists { get; set; }
}

public class CurrentArtist
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}