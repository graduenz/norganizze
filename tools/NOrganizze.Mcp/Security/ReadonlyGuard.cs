using NOrganizze.Mcp.Configuration;

namespace NOrganizze.Mcp.Security;

public sealed class ReadonlyGuard
{
    private readonly NOrganizzeMcpConfiguration _config;

    public ReadonlyGuard(NOrganizzeMcpConfiguration config)
    {
        _config = config;
    }

    public void EnsureWriteAllowed(string toolName)
    {
        if (!_config.Readonly)
            return;

        throw new InvalidOperationException($"Tool '{toolName}' is blocked because NORGANIZZE MCP is running in readonly mode.");
    }
}
