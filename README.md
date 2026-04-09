# NOrganizze

[![NuGet](https://img.shields.io/nuget/v/NOrganizze)](https://www.nuget.org/packages/NOrganizze)
[![NuGet](https://img.shields.io/nuget/dt/NOrganizze?color=blue)](https://www.nuget.org/packages/NOrganizze)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=graduenz_norganizze&metric=bugs)](https://sonarcloud.io/summary/new_code?id=graduenz_norganizze)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=graduenz_norganizze&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=graduenz_norganizze)

<img src="logo/logo-bg-transparent.png" alt="NOrganizze logo" width="100">

---

**NOrganizze** is an unofficial, strongly-typed .NET client for the Organizze API.

[Organizze](https://www.organizze.com.br/) is a personal finance management tool that helps users track their expenses, manage budgets, and gain insights into their financial habits. Their API is documented at [Organizze API Documentation](https://github.com/organizze/api-doc).

## Usage

For detailed API usage, including how to create an `NOrganizzeClient`, filter transactions with `TransactionListOptions`, and use all services and option types, see the [Usage guide](USAGE.md).

## Versioning

- Repository-wide project version metadata is centralized in `Directory.Build.props`.
- Repository-wide NuGet package dependency versions are centralized in `Directory.Packages.props` via Central Package Management.
- NuGet release versions are tag-driven in CI (`vX.Y.Z...`) and passed during pack.

## MCP Server

![Docker Image Version](https://img.shields.io/docker/v/graduenz/norganizze-mcp)

This repository includes a Docker-friendly MCP server at `tools/NOrganizze.Mcp`.

- Config precedence: `.mcp-norganizze.json` first, then `NORGANIZZE_*` environment variables.
- `readonly` is enabled by default and blocks all mutating tools (`create`, `update`, `delete`) server-side.
- Docker Hub image: `docker.io/graduenz/norganizze-mcp` (release tags publish matching image tags).

See the [MCP server guide](tools/NOrganizze.Mcp/README.md) for build/run instructions and a Cursor MCP config example.

## Known limitations

The Organizze API returns a maximum of **500 transactions per request** (`TransactionService.MaxTransactionsPerRequest`). When listing transactions, NOrganizze automatically paginates by advancing the `start_date` to the latest transaction date from each batch, deduplicating by transaction id, and repeating until fewer than 500 results are returned (controlled by `TransactionListOptions.AutoPaginate`, which defaults to `true`). When `StartDate` or `EndDate` are not provided, the library defaults to the current month boundaries.

If a single **date** contains more than 500 transactions, the API itself truncates the response and there is no known workaround at the API level.

Set `AutoPaginate = false` to opt out and make a single API call (original behavior). See the [Usage guide](USAGE.md) for details and examples.

> **Note on date filtering:** The official Organizze API documentation states that `start_date` and `end_date` are always rounded to full calendar months. Empirical testing shows this is inaccurate -- the API **does respect exact dates** when explicitly provided. The "full month" behavior only applies as a default when these parameters are omitted entirely.

## License

[Apache-2.0](./LICENSE)
