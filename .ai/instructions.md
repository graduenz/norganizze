# Agent Instructions — NOrganizze

This is the entrypoint for all AI agent context in the NOrganizze repository. Read this file first, then consult the referenced documents as needed.

---

## Context Map

| File | Purpose |
|------|---------|
| `.ai/instructions.md` | This file — entrypoint, context map, core mandate, and documentation protocol |
| `.ai/norganizze.md` | Project-specific rules: architecture, repository layout, target frameworks, development commands, tests & secrets, CI/CD, MCP server conventions, Organizze API behavior, and agent/contributor conventions |
| `.ai/dotnet.md` | General .NET/C# development rules: code style, naming conventions, patterns, testing, and API design best practices |
| `.ai/testing.md` | Testing-specific rules: xUnit v3 conventions, test naming pattern, AAA structure, Moq/Bogus usage, and test verification commands |

---

## Core Mandate

**NOrganizze** is an unofficial, strongly-typed .NET client library for the [Organizze](https://www.organizze.com.br/) REST API. It is published as the NuGet package **NOrganizze** under the Apache-2.0 license. Repository: `https://github.com/graduenz/norganizze`.

### What this project is

- A .NET HTTP client library that wraps the Organizze REST API with strongly-typed models and service classes.
- Distributed as a NuGet package targeting `net8.0`, `net9.0`, `net10.0`, `netstandard2.0`, and `net472`.
- Accompanied by an MCP server (`tools/NOrganizze.Mcp`) that exposes client operations as MCP tools for agent workflows, distributed as a Docker image.
- Accompanied by an `ApiValidator` tool that validates models and endpoints against the real API and generates empirical OpenAPI specs.

### Non-negotiable constraints

- **Multi-target compatibility is mandatory.** Every change must work across all target frameworks. JSON serialization uses System.Text.Json on `net8.0+` and Newtonsoft.Json on `netstandard2.0`/`net472`; use `#if NET8_0_OR_GREATER` guards where paths diverge.
- **`dotnet build` is the standard verification step.** Tests require live Organizze credentials and are run manually — never assume they can be run in CI or agent environments.
- **Project-specific rules (`.ai/norganizze.md`) take precedence** over general .NET rules (`.ai/dotnet.md`) in case of any conflict.
- **Never commit real credentials.** Credentials are managed exclusively via .NET user secrets.
- **Keep XML doc comments on all public API surface** — the library sets `GenerateDocumentationFile`.
- **MCP server must stay aligned with the main client API surface.** When library features change, update MCP tools accordingly.

---

## Documentation Protocol

The `.ai/` directory is the single source of truth for agent and contributor guidance. As the project evolves, agents and contributors **must** keep these documents synchronized with the codebase. Stale rules are worse than no rules.

### When to update `.ai/` documents

| Change | Files to update |
|--------|----------------|
| New service, endpoint, or model added | `.ai/norganizze.md` — architecture section; MCP conventions if applicable |
| Framework targets added or removed | `.ai/norganizze.md` — target frameworks section |
| New dev command, tool, or CI workflow | `.ai/norganizze.md` — relevant section |
| General .NET pattern or testing convention changed | `.ai/dotnet.md` |
| New `.ai/` document created | `.ai/instructions.md` — Context Map table |
| Significant refactor or architectural shift | Review all `.ai/` documents for accuracy; update any section that no longer reflects reality |

### Update principles

- **Prefer small, precise updates** over large rewrites — target only the sections affected by the change.
- **After any large change**, review the entire relevant `.ai/` document for consistency, not just the most obviously affected section.
- **New `.ai/` documents must be registered** in the Context Map above before being committed.
- **When in doubt, update the rule.** Rules are cheap; confusion is expensive.
- **Never leave the codebase and `.ai/` rules out of sync.** Treat documentation drift as a bug.
