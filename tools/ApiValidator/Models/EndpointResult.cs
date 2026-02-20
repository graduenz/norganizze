namespace ApiValidator.Models;

public class EndpointResult
{
    public string Service { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public bool Success { get; set; }
    public int Attempts { get; set; }
    public int StatusCode { get; set; }
    public long ResponseTimeMs { get; set; }
    public ValidationResult Validation { get; set; } = new();
    public string? RawRequest { get; set; }
    public string? RawResponse { get; set; }
    public string? ErrorMessage { get; set; }
}

public class ValidationResult
{
    public bool Passed { get; set; }
    public List<string> Errors { get; set; } = new();
    public List<PropertyDiscrepancy> Discrepancies { get; set; } = new();
}

public class PropertyDiscrepancy
{
    public string PropertyName { get; set; } = string.Empty;
    public string Issue { get; set; } = string.Empty;
    public string? ExpectedType { get; set; }
    public string? ActualType { get; set; }
}

public class ApiValidationReport
{
    public ReportMetadata Metadata { get; set; } = new();
    public List<EndpointResult> Endpoints { get; set; } = new();
    public ReportSummary Summary { get; set; } = new();
    public List<OpenApiChange> Changes { get; set; } = new();
}

public class ReportMetadata
{
    public DateTime Timestamp { get; set; }
    public string Email { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public int TotalEndpointsTested { get; set; }
}

public class ReportSummary
{
    public int Total { get; set; }
    public int Succeeded { get; set; }
    public int Failed { get; set; }
    public int DiscrepancyCount { get; set; }
}

public class OpenApiChange
{
    public string Endpoint { get; set; } = string.Empty;
    public string ChangeType { get; set; } = string.Empty; // UNCHANGED, MODIFIED, FAILED
    public List<string> Details { get; set; } = new();
}
