using ApiValidator.Models;
using ApiValidator.Services;
using Microsoft.Extensions.Configuration;

namespace ApiValidator;

static class Program
{
    private const string Separator = "═══════════════════════════════════════════════════════";

    static async Task Main(string[] args)
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║        NOrganizze API Validator & OpenAPI Generator   ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝");
        Console.WriteLine();

        // Load configuration from user secrets
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets(typeof(Program).Assembly)
            .Build();

        var email = configuration["Organizze:Email"];
        var apiToken = configuration["Organizze:ApiKey"];

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(apiToken))
        {
            Console.WriteLine("❌ Error: Credentials not found in user secrets.");
            Console.WriteLine();
            Console.WriteLine("Please configure user secrets with:");
            Console.WriteLine("  dotnet user-secrets set \"Organizze:Email\" \"your-email@example.com\"");
            Console.WriteLine("  dotnet user-secrets set \"Organizze:ApiKey\" \"your-api-key\"");
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            Environment.Exit(1);
            return;
        }

        var userAgent = $"NOrganizze API Validator ({email})";

        Console.WriteLine($"Email: {email}");
        Console.WriteLine($"User Agent: {userAgent}");
        Console.WriteLine();
        Console.WriteLine("⚠️  This will create and delete test data in your Organizze account.");
        Console.WriteLine("Press any key to continue or Ctrl+C to cancel...");
        Console.ReadKey();
        Console.WriteLine();

        try
        {
            // Execute all tests
            var orchestrator = new ApiTestOrchestrator(email, apiToken, userAgent);
            var results = await orchestrator.ExecuteAllTestsAsync();

            Console.WriteLine();
            Console.WriteLine(Separator);
            Console.WriteLine("Test Execution Complete!");
            Console.WriteLine(Separator);
            Console.WriteLine();

            // Build report
            var report = BuildReport(results, email, userAgent);

            // Generate reports
            var projectRoot = GetProjectRoot();

            // Setup output directories under tools/ApiValidator/output
            var validatorDir = Path.Combine(projectRoot, "tools", "ApiValidator");
            var outputDir = Path.Combine(validatorDir, "output");
            var reportsDir = Path.Combine(outputDir, "reports");
            var specsDir = Path.Combine(outputDir, "specs");

            Directory.CreateDirectory(reportsDir);
            Directory.CreateDirectory(specsDir);

            // Generate reports
            var markdownPath = Path.Combine(reportsDir, "api-validation-report.md");
            ReportGenerator.SaveMarkdownReport(report, markdownPath);
            Console.WriteLine($"✅ Markdown report: {markdownPath}");

            var jsonPath = Path.Combine(reportsDir, "api-validation-report.json");
            ReportGenerator.SaveJsonReport(report, jsonPath);
            Console.WriteLine($"✅ JSON report: {jsonPath}");

            Console.WriteLine();

            // Generate empirical OpenAPI spec
            var openApiGen = new OpenApiGenerator();
            openApiGen.GenerateFromResults(results);

            var openApiPath = Path.Combine(specsDir, "organizze-api-openapi-empirical.yaml");
            openApiGen.SaveToYaml(openApiPath);

            Console.WriteLine($"✅ Empirical OpenAPI spec: {openApiPath}");
            Console.WriteLine();

            // Display summary
            Console.WriteLine(Separator);
            Console.WriteLine("Summary:");
            Console.WriteLine(Separator);
            Console.WriteLine($"Total Endpoints Tested: {report.Summary.Total}");
            Console.WriteLine($"Successful: {report.Summary.Succeeded} ✅");
            Console.WriteLine($"Failed: {report.Summary.Failed} ❌");
            Console.WriteLine($"Validation Discrepancies: {report.Summary.DiscrepancyCount} ⚠️");

            var successRate = report.Summary.Total > 0
                ? (double)report.Summary.Succeeded / report.Summary.Total * 100
                : 0;
            Console.WriteLine($"Success Rate: {successRate:F1}%");
            Console.WriteLine();

            if (report.Summary.DiscrepancyCount > 0)
            {
                Console.WriteLine("⚠️  Some validation discrepancies were found.");
                Console.WriteLine("   Check the reports for details on C# model vs API response differences.");
                Console.WriteLine();
            }

            if (report.Summary.Failed > 0)
            {
                Console.WriteLine("❌ Some endpoints failed to execute.");
                Console.WriteLine("   Check the reports for error details.");
                Console.WriteLine();
            }

            Console.WriteLine(Separator);
            Console.WriteLine("Files Generated:");
            Console.WriteLine(Separator);
            Console.WriteLine($"1. tools/ApiValidator/output/reports/{Path.GetFileName(markdownPath)}");
            Console.WriteLine($"   Human-readable validation report");
            Console.WriteLine($"2. tools/ApiValidator/output/reports/{Path.GetFileName(jsonPath)}");
            Console.WriteLine($"   Machine-readable validation data");
            Console.WriteLine($"3. tools/ApiValidator/output/specs/{Path.GetFileName(openApiPath)}");
            Console.WriteLine($"   Empirical OpenAPI specification");
            Console.WriteLine();
            Console.WriteLine("Compare with existing: organizze-api-openapi.yaml (in root)");
            Console.WriteLine();
            Console.WriteLine("✨ Validation complete! Press any key to exit...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(Separator);
            Console.WriteLine("❌ FATAL ERROR");
            Console.WriteLine(Separator);
            Console.WriteLine(ex.ToString());
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            Environment.Exit(1);
        }
    }

    static ApiValidationReport BuildReport(List<EndpointResult> results, string email, string userAgent)
    {
        var report = new ApiValidationReport();

        report.Metadata = new ReportMetadata
        {
            Timestamp = DateTime.UtcNow,
            Email = email,
            UserAgent = userAgent,
            TotalEndpointsTested = results.Count
        };

        report.Endpoints = results;

        report.Summary = new ReportSummary
        {
            Total = results.Count,
            Succeeded = results.Count(r => r.Success),
            Failed = results.Count(r => !r.Success),
            DiscrepancyCount = results.Sum(r => r.Validation.Discrepancies.Count)
        };

        // Analyze changes (simplified version - could be enhanced to compare with existing OpenAPI)
        foreach (var result in results)
        {
            var change = new OpenApiChange
            {
                Endpoint = $"{result.Method} {result.Path}"
            };

            if (!result.Success)
            {
                change.ChangeType = "FAILED";
                change.Details.Add($"Endpoint failed: {result.ErrorMessage}");
            }
            else if (result.Validation.Discrepancies.Count > 0)
            {
                change.ChangeType = "MODIFIED";
                foreach (var disc in result.Validation.Discrepancies)
                {
                    change.Details.Add($"{disc.PropertyName}: {disc.Issue}");
                }
            }
            else
            {
                change.ChangeType = "UNCHANGED";
            }

            report.Changes.Add(change);
        }

        return report;
    }

    static string GetProjectRoot()
    {
        var currentDir = Directory.GetCurrentDirectory();

        // Navigate up to find the solution root (contains .slnx file)
        while (!string.IsNullOrEmpty(currentDir))
        {
            if (File.Exists(Path.Combine(currentDir, "NOrganizze.slnx")) ||
                File.Exists(Path.Combine(currentDir, "global.json")))
            {
                return currentDir;
            }

            var parent = Directory.GetParent(currentDir);
            if (parent == null) break;
            currentDir = parent.FullName;
        }

        // Fallback to current directory
        return Directory.GetCurrentDirectory();
    }
}
