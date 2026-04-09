using System.Text.Json;

namespace NOrganizze.Mcp.Configuration;

public static class NOrganizzeMcpConfigurationLoader
{
    private const string ConfigFileName = ".mcp-norganizze.json";
    private const string ConfigPathEnv = "NORGANIZZE_CONFIG_PATH";
    private const string EmailEnv = "NORGANIZZE_EMAIL";
    private const string ApiKeyEnv = "NORGANIZZE_API_KEY";
    private const string NameEnv = "NORGANIZZE_NAME";
    private const string ReadonlyEnv = "NORGANIZZE_READONLY";
    private const string BaseUrlEnv = "NORGANIZZE_BASE_URL";

    public static NOrganizzeMcpConfiguration Load()
    {
        var fromFile = TryLoadFromFile();
        var config = new NOrganizzeMcpConfiguration
        {
            Email = FirstNonEmpty(fromFile?.Email, Environment.GetEnvironmentVariable(EmailEnv)),
            ApiKey = FirstNonEmpty(fromFile?.ApiKey, Environment.GetEnvironmentVariable(ApiKeyEnv)),
            Name = FirstNonEmpty(fromFile?.Name, Environment.GetEnvironmentVariable(NameEnv)),
            BaseUrl = FirstNonEmpty(fromFile?.BaseUrl, Environment.GetEnvironmentVariable(BaseUrlEnv), NOrganizzeClient.OrganizzeRestV2Url),
            Readonly = ResolveReadonly(fromFile)
        };

        if (string.IsNullOrWhiteSpace(config.Email))
            throw new InvalidOperationException("NOrganizze MCP configuration error: missing email. Provide it in .mcp-norganizze.json or NORGANIZZE_EMAIL.");

        if (string.IsNullOrWhiteSpace(config.ApiKey))
            throw new InvalidOperationException("NOrganizze MCP configuration error: missing apiKey. Provide it in .mcp-norganizze.json or NORGANIZZE_API_KEY.");

        return config;
    }

    private static NOrganizzeMcpConfiguration TryLoadFromFile()
    {
        var explicitPath = Environment.GetEnvironmentVariable(ConfigPathEnv);
        var path = string.IsNullOrWhiteSpace(explicitPath)
            ? Path.Combine(Environment.CurrentDirectory, ConfigFileName)
            : explicitPath;

        if (!File.Exists(path))
            return null;

        var content = File.ReadAllText(path);
        if (string.IsNullOrWhiteSpace(content))
            return null;

        return JsonSerializer.Deserialize<NOrganizzeMcpConfiguration>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    private static bool ResolveReadonly(NOrganizzeMcpConfiguration fromFile)
    {
        if (fromFile != null)
            return fromFile.Readonly;

        var raw = Environment.GetEnvironmentVariable(ReadonlyEnv);
        if (string.IsNullOrWhiteSpace(raw))
            return true;

        if (bool.TryParse(raw, out var parsed))
            return parsed;

        return true;
    }

    private static string FirstNonEmpty(params string[] values)
    {
        foreach (var value in values)
        {
            if (!string.IsNullOrWhiteSpace(value))
                return value;
        }

        return null;
    }
}
