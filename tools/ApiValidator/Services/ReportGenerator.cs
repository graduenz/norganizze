using System.Text;
using ApiValidator.Models;

namespace ApiValidator.Services;

public static class ReportGenerator
{
    public static string GenerateMarkdownReport(ApiValidationReport report)
    {
        var sb = new StringBuilder();

        AppendHeader(report, sb);
        AppendExecutiveSummary(report, sb);
        AppendTestResultsByService(report, sb);
        AppendValidationDiscrepancies(report, sb);
        AppendFailedEndpoints(report, sb);
        AppendOpenApiChanges(report, sb);
        AppendPerformanceStatistics(report, sb);
        AppendFooter(sb);

        return sb.ToString();
    }

    private static void AppendHeader(ApiValidationReport report, StringBuilder sb)
    {
        sb.AppendLine("# NOrganizze API Validation Report");
        sb.AppendLine();
        sb.AppendLine($"**Generated:** {report.Metadata.Timestamp:yyyy-MM-dd HH:mm:ss} (UTC)");
        sb.AppendLine($"**Email:** {report.Metadata.Email}");
        sb.AppendLine($"**User Agent:** {report.Metadata.UserAgent}");
        sb.AppendLine();
    }

    private static void AppendExecutiveSummary(ApiValidationReport report, StringBuilder sb)
    {
        sb.AppendLine("## Executive Summary");
        sb.AppendLine();
        sb.AppendLine($"- **Total Endpoints Tested:** {report.Summary.Total}");
        sb.AppendLine($"- **Successful:** {report.Summary.Succeeded} ✅");
        sb.AppendLine($"- **Failed:** {report.Summary.Failed} ❌");
        sb.AppendLine($"- **Validation Discrepancies:** {report.Summary.DiscrepancyCount} ⚠️");
        sb.AppendLine();

        var successRate = report.Summary.Total > 0
            ? (double)report.Summary.Succeeded / report.Summary.Total * 100
            : 0;
        sb.AppendLine($"**Success Rate:** {successRate:F1}%");
        sb.AppendLine();
    }

    private static void AppendTestResultsByService(ApiValidationReport report, StringBuilder sb)
    {
        sb.AppendLine("## Test Results by Service");
        sb.AppendLine();

        var serviceGroups = report.Endpoints.GroupBy(e => e.Service).OrderBy(g => g.Key);
        foreach (var group in serviceGroups)
        {
            sb.AppendLine($"### {group.Key}");
            sb.AppendLine();
            sb.AppendLine("| Status | Method | Path | Attempts | Time (ms) | Validation |");
            sb.AppendLine("|--------|--------|------|----------|-----------|------------|");

            foreach (var endpoint in group.OrderBy(e => e.Path))
            {
                var status = endpoint.Success ? "✅" : "❌";
                var validation = GetValidationSymbol(endpoint);
                sb.AppendLine($"| {status} | {endpoint.Method} | `{endpoint.Path}` | {endpoint.Attempts} | {endpoint.ResponseTimeMs} | {validation} |");
            }

            sb.AppendLine();
        }
    }

    private static string GetValidationSymbol(EndpointResult endpoint)
    {
        if (endpoint.Validation.Passed) return "✅";
        return endpoint.Validation.Discrepancies.Count > 0 ? "⚠️" : "❌";
    }

    private static void AppendValidationDiscrepancies(ApiValidationReport report, StringBuilder sb)
    {
        if (report.Summary.DiscrepancyCount == 0) return;

        sb.AppendLine("## Validation Discrepancies");
        sb.AppendLine();
        sb.AppendLine("These are differences between the C# models and the actual API responses:");
        sb.AppendLine();

        foreach (var endpoint in report.Endpoints.Where(e => e.Validation.Discrepancies.Count > 0))
        {
            sb.AppendLine($"### {endpoint.Method} {endpoint.Path}");
            sb.AppendLine();

            foreach (var discrepancy in endpoint.Validation.Discrepancies)
            {
                sb.AppendLine($"- **{discrepancy.PropertyName}**: {discrepancy.Issue}");
                if (discrepancy.ExpectedType != null || discrepancy.ActualType != null)
                {
                    sb.AppendLine($"  - Expected: `{discrepancy.ExpectedType ?? "N/A"}`");
                    sb.AppendLine($"  - Actual: `{discrepancy.ActualType ?? "N/A"}`");
                }
            }

            sb.AppendLine();
        }
    }

    private static void AppendFailedEndpoints(ApiValidationReport report, StringBuilder sb)
    {
        var failedEndpoints = report.Endpoints.Where(e => !e.Success).ToList();
        if (failedEndpoints.Count == 0) return;

        sb.AppendLine("## Failed Endpoints");
        sb.AppendLine();
        sb.AppendLine("These endpoints failed to execute successfully:");
        sb.AppendLine();

        foreach (var endpoint in failedEndpoints)
        {
            sb.AppendLine($"### {endpoint.Method} {endpoint.Path}");
            sb.AppendLine();
            sb.AppendLine($"**Error:** {endpoint.ErrorMessage}");
            sb.AppendLine();

            if (endpoint.Validation.Errors.Count > 0)
            {
                sb.AppendLine("**Validation Errors:**");
                foreach (var error in endpoint.Validation.Errors)
                {
                    sb.AppendLine($"- {error}");
                }
                sb.AppendLine();
            }
        }
    }

    private static void AppendOpenApiChanges(ApiValidationReport report, StringBuilder sb)
    {
        if (report.Changes.Count == 0) return;

        sb.AppendLine("## OpenAPI Specification Changes");
        sb.AppendLine();
        sb.AppendLine("Comparison between documented OpenAPI spec and empirical observations:");
        sb.AppendLine();

        var unchanged = report.Changes.Count(c => c.ChangeType == "UNCHANGED");
        var modified = report.Changes.Count(c => c.ChangeType == "MODIFIED");
        var failed = report.Changes.Count(c => c.ChangeType == "FAILED");

        sb.AppendLine($"- **Unchanged:** {unchanged}");
        sb.AppendLine($"- **Modified:** {modified}");
        sb.AppendLine($"- **Failed:** {failed}");
        sb.AppendLine();

        var modifiedChanges = report.Changes.Where(c => c.ChangeType == "MODIFIED").ToList();
        if (modifiedChanges.Count > 0)
        {
            sb.AppendLine("### Modified Endpoints");
            sb.AppendLine();

            foreach (var change in modifiedChanges)
            {
                sb.AppendLine($"#### {change.Endpoint}");
                sb.AppendLine();
                foreach (var detail in change.Details)
                {
                    sb.AppendLine($"- {detail}");
                }
                sb.AppendLine();
            }
        }
    }

    private static void AppendPerformanceStatistics(ApiValidationReport report, StringBuilder sb)
    {
        sb.AppendLine("## Performance Statistics");
        sb.AppendLine();

        var successfulEndpoints = report.Endpoints.Where(e => e.Success).ToList();
        if (successfulEndpoints.Count > 0)
        {
            var avgTime = successfulEndpoints.Average(e => e.ResponseTimeMs);
            var maxTime = successfulEndpoints.Max(e => e.ResponseTimeMs);
            var minTime = successfulEndpoints.Min(e => e.ResponseTimeMs);

            sb.AppendLine($"- **Average Response Time:** {avgTime:F0}ms");
            sb.AppendLine($"- **Min Response Time:** {minTime}ms");
            sb.AppendLine($"- **Max Response Time:** {maxTime}ms");
            sb.AppendLine();

            var retriedEndpoints = successfulEndpoints.Count(e => e.Attempts > 1);
            sb.AppendLine($"- **Endpoints Requiring Retry:** {retriedEndpoints}");
        }

        sb.AppendLine();
    }

    private static void AppendFooter(StringBuilder sb)
    {
        sb.AppendLine("---");
        sb.AppendLine();
        sb.AppendLine("*Report generated by NOrganizze API Validator*");
    }

    public static void SaveMarkdownReport(ApiValidationReport report, string filePath)
    {
        var markdown = GenerateMarkdownReport(report);
        File.WriteAllText(filePath, markdown);
    }

    public static void SaveJsonReport(ApiValidationReport report, string filePath)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(report, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
        });
        File.WriteAllText(filePath, json);
    }
}
