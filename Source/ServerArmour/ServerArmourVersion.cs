using System.Text.Json.Serialization;

namespace ServerAmrour;

public class ServerArmourVersion
{
    [JsonPropertyName("major")]
    public int Major { get; set; }

    [JsonPropertyName("minor")]
    public int Minor { get; set; }

    [JsonPropertyName("patch")]
    public int Patch { get; set; }
    public override string ToString()
    {
        return $"{Major}.{Minor}.{Patch}";
    }
}