using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using NOrganizze;
using NOrganizze.Mcp.Configuration;
using NOrganizze.Mcp.Security;

var builder = Host.CreateApplicationBuilder(args);
var config = NOrganizzeMcpConfigurationLoader.Load();

builder.Logging.AddConsole(options =>
{
    options.LogToStandardErrorThreshold = LogLevel.Trace;
});

builder.Services.AddSingleton(config);
builder.Services.AddSingleton<ReadonlyGuard>();
builder.Services.AddSingleton(serviceProvider =>
{
    var config = serviceProvider.GetRequiredService<NOrganizzeMcpConfiguration>();
    var client = new NOrganizzeClient(config.Email, config.ApiKey, config.BaseUrl);
    return client;
});

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

await builder.Build().RunAsync();
