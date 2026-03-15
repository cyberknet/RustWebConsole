using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ServerAmrour;

public class ServerArmourPlugin
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("author")]
    public string Author { get; set; }

    [JsonPropertyName("marketplace")]
    public string Marketplace { get; set; }

    [JsonIgnore(Condition=JsonIgnoreCondition.WhenWriting)]
    [JsonPropertyName("tags")]
    public string Tags { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("downloadUrl")]
    public string DownloadUrl { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
    [JsonPropertyName("iconUrl")]
    public string IconUrl { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
    [JsonPropertyName("downloads")]
    public long Downloads { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
    [JsonPropertyName("rating")]
    public Decimal Rating { get; set; }
    
    [JsonPropertyName("latestVersion")]
    public string LatestVersion { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
    public ServerArmourVersion LatestVersionSemantic { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
    public ServerArmourMatch Match { get; set; }
}
