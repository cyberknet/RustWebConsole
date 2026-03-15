using System;
using System.Net.Http;
using System.Net;
using System.Text.Json;
using System.Collections.Generic;
using System.Text;

namespace ServerAmrour;

public class ServerArmourClient(string urlBase)
{
    public string UrlBase { get; set; } = urlBase;

    // Shared logic for querying the API and parsing the response
    private ServerArmourResponse? Query(string name)
    {
        try
        {
            char[] arr = name.ToCharArray();

            arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c)
                                              || char.IsWhiteSpace(c)
                                              || c == '-')));
            name = new string(arr);

            string url = UrlBase + WebUtility.UrlEncode(name);
            using var httpClient = new HttpClient();
            var response = httpClient.GetAsync(url).Result;
            if (!response.IsSuccessStatusCode)
                return null;
            var json = response.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<ServerArmourResponse>(json);
        }
        catch
        {
            return null;
        }
    }

    public List<ServerArmourPlugin> Search(string name)
    {
        var result = Query(name);
        if (result != null && result.Status == 200 && result.Data != null && result.Data.Count > 0)
            return result.Data;
        return [];
    }

    public ServerArmourPlugin? Get(string name, Guid id)
    {
        var result = Query(name);
        if (result != null && result.Status == 200 && result.Data != null && result.Data.Count > 0)
            return result.Data.FirstOrDefault(p => p.Id == id);
        return null;
    }
}
