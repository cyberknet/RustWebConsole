using System.Text.Json.Serialization;

namespace ServerAmrour;

public class ServerArmourMatch
{
    [JsonPropertyName("name")]
    public decimal Name { get; set; }

    [JsonPropertyName("tags")]
    public decimal Tags { get; set; }

    [JsonPropertyName("slug")]
    public decimal Slug { get; set; }

    [JsonPropertyName("manualName")]
    public decimal ManualName { get; set; }

    [JsonPropertyName("filename")]
    public decimal Filename { get; set; }
}
