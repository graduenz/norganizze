namespace NOrganizze.Mcp.Configuration;

public sealed class NOrganizzeMcpConfiguration
{
    public string Email { get; set; }

    public string ApiKey { get; set; }

    public string Name { get; set; }

    public bool Readonly { get; set; } = true;

    public string BaseUrl { get; set; } = NOrganizzeClient.OrganizzeRestV2Url;
}
