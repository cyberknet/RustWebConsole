using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ServerAmrour;

public class ServerArmourResponse
{
    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("data")]
    public List<ServerArmourPlugin> Data { get; set; }
    
    [JsonPropertyName("isArray")]
    public bool IsArray { get; set; }
}
