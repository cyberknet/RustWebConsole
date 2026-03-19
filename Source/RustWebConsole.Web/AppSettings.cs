using AutoMapper.Internal;
using Microsoft.AspNetCore.Identity;

namespace RustWebConsole.Web;

public class AppSettings
{
    // maps the property names in our AppSettings class to their corresponding environment variable names
    public static Dictionary<string, string> EnvironmentMap { get; set; } = new()
    {
        {"Identity:PasswordRequireDigit",               "RWC_IDENTITY_PASSWORDREQUIREDIGIT" },
        {"Identity:PasswordRequiredLength",             "RWC_IDENTITY_PASSWORDREQUIREDLENGTH"  },
        {"Identity:PasswordRequireNonAlphanumeric",     "RWC_IDENTITY_PASSWORDREQUIRENONALPHANUMERIC" },
        {"Identity:PasswordRequireUppercase",           "RWC_IDENTITY_PASSWORDREQUIREUPPERCASE" },
        {"Identity:PasswordRequireLowercase",           "RWC_IDENTITY_PASSWORDREQUIRELOWERCASE" },
        {"Identity:RequireConfirmedAccount",    "RWC_IDENTITY_REQUIRECONFIRMEDACCOUNT" },

        {"Jwt:ValidIssuer",                     "RWC_JWT_VALIDISSUER" },
        {"Jwt:ValidAudience",                   "RWC_JWT_VALIDAUDIENCE" },
        {"Jwt:IssuerSigningKey",                "RWC_JWT_ISSUERSIGNINGKEY" },

        {"ConnectionStrings:DefaultConnection", "RWC_CONNECTIONS_DEFAULT" }
    };
    public AppSettings()
    {
        Jwt = new ();
        ConnectionStrings = new ();
        Identity = new ();
    }

    public Identity Identity { get; set; }
    public JwtSettings Jwt { get; private set; }
    public ConnectionStrings ConnectionStrings { get; private set; }
}

public class Identity
{
    public bool PasswordRequireDigit { get; set; } = true;
    public int PasswordRequiredLength { get; set; } = 6;
    public bool PasswordRequireNonAlphanumeric { get; set; } = true;
    public bool PasswordRequireUppercase { get; set; } = true;
    public bool PasswordRequireLowercase { get; set; } = true;
    public bool RequireConfirmedAccount { get; set; } = true;
}

public class JwtSettings
{
    public string ValidIssuer { get; set; } = string.Empty;
    public string ValidAudience { get; set; } = string.Empty;
    public string IssuerSigningKey { get; set; } = string.Empty;
}

public class ConnectionStrings
{
    public string DefaultConnection { get; set; } = string.Empty;
}

public static class AppSettingsHelper
{
    public static T GetConfigValue<T>(
        IConfiguration configuration, 
        string propertyName, 
        Dictionary<string, (string EnvironmentVariableName, string AppSettingsKey)> keys, 
        T defaultValue) where T : IParsable<T>
    {
        // get our key info for the property
        var keyInfo = keys[propertyName];

        // Get the raw string from Env Var, if missing then from AppSettings
        string? stringValue = Environment.GetEnvironmentVariable(keyInfo.EnvironmentVariableName)
                              ?? configuration[keyInfo.AppSettingsKey];

        // if no value at all was found
        if (string.IsNullOrWhiteSpace(stringValue))
            return defaultValue;

        // Call TryParse directly on the generic type T
        // No boxing, no if/else, and no (object) casts!
        if (T.TryParse(stringValue, null, out var result))
        {
            return result;
        }

        return defaultValue;
    }

    public static string GetConfigValue(
        IConfiguration configuration,
        string propertyName,
        Dictionary<string, (string EnvironmentVariableName, string AppSettingsKey)> keys,
        string defaultValue)
    {
        var keyInfo = keys[propertyName];
        return Environment.GetEnvironmentVariable(keyInfo.EnvironmentVariableName)
               ?? configuration[keyInfo.AppSettingsKey]
               ?? defaultValue;
    }

}