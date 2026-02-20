# API Validator

A comprehensive validation tool for the NOrganizze library that tests all endpoints against the actual Organizze API.

## Purpose

This tool:
- Verifies that C# models accurately represent API responses
- Detects API changes or discrepancies
- Generates an empirical OpenAPI specification based on real responses
- Validates request/response compatibility
- Tests all CRUD operations with automatic cleanup

## Running the Validator

From the repository root:

```bash
cd tools/ApiValidator
dotnet run
```

Or directly:

```bash
dotnet run --project tools/ApiValidator
```

## Prerequisites

The validator requires user secrets to be configured (shared with the test project):

```bash
cd tools/ApiValidator
dotnet user-secrets set "Organizze:Email" "your-email@example.com"
dotnet user-secrets set "Organizze:ApiKey" "your-api-key"
```

## How It Works

The validator executes tests in 6 phases:

1. **READ operations** - Tests existing data (accounts, categories, budgets, etc.)
2. **CREATE test data** - Creates temporary accounts, categories, and credit cards
3. **CREATE dependent data** - Creates transactions and transfers using test data
4. **UPDATE operations** - Tests modification of created entities
5. **READ created data** - Verifies changes persisted correctly
6. **DELETE cleanup** - Removes all test data in reverse dependency order

## Output

Generated files are saved in the `output/` subfolder:

### `output/reports/`
- **`api-validation-report.md`** - Human-readable report with test results, discrepancies, and performance stats
- **`api-validation-report.json`** - Machine-readable report for programmatic analysis

### `output/specs/`
- **`organizze-api-openapi-empirical.yaml`** - Empirically generated OpenAPI spec from actual API responses

## Git Tracking

Output files can be committed to track API validation history, or ignored by uncommenting the entry in `.gitignore`:

```gitignore
# tools/ApiValidator/output/
```
