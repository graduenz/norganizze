# NOrganizze MCP Server

MCP server for the NOrganizze .NET client.

## Features

- Full tool surface across Users, Accounts, Categories, Credit Cards, Invoices, Transactions, Transfers, and Budgets.
- Readonly mode is enabled by default.
- Credentials are loaded from `.mcp-norganizze.json` first, then `NORGANIZZE_*` environment variables.

## Configuration

The server looks for configuration in this order:

1. `.mcp-norganizze.json` in the current working directory (or path from `NORGANIZZE_CONFIG_PATH`).
2. Environment variables:
   - `NORGANIZZE_EMAIL` (required if not in file)
   - `NORGANIZZE_API_KEY` (required if not in file)
   - `NORGANIZZE_NAME` (optional)
   - `NORGANIZZE_READONLY` (optional, default: `true`)
   - `NORGANIZZE_BASE_URL` (optional)

Example file:

```json
{
  "email": "you@example.com",
  "apiKey": "your-api-key",
  "name": "NOrganizze MCP",
  "readonly": true,
  "baseUrl": "https://api.organizze.com.br/rest/v2"
}
```

## Readonly behavior

When readonly is true, all mutating tools (`create`, `update`, `delete`) return an error.
Read tools (`list`, `get`) continue to work.

## Docker

Published image:

```bash
docker.io/graduenz/norganizze-mcp
```

Tag policy:

- Stable release tags (`vX.Y.Z`) publish `X.Y.Z` and `latest`.
- Prerelease tags (`vX.Y.Z-suffix`) publish only `X.Y.Z-suffix`.

Build image from repository root:

```bash
docker build -f tools/NOrganizze.Mcp/Dockerfile -t graduenz/norganizze-mcp:latest .
```

Run with workspace-mounted config:

```bash
docker run --rm -i \
  -v "$PWD:/workspace" \
  -w /workspace \
  docker.io/graduenz/norganizze-mcp:latest
```

## Cursor MCP configuration (example)

Use Docker stdio transport:

```json
{
  "mcpServers": {
    "norganizze": {
      "command": "docker",
      "args": [
        "run",
        "--rm",
        "-i",
        "-v",
        "${workspaceFolder}:/workspace",
        "-w",
        "/workspace",
        "-e",
        "NORGANIZZE_EMAIL",
        "-e",
        "NORGANIZZE_API_KEY",
        "-e",
        "NORGANIZZE_NAME",
        "-e",
        "NORGANIZZE_READONLY",
        "-e",
        "NORGANIZZE_BASE_URL",
        "docker.io/graduenz/norganizze-mcp:latest"
      ]
    }
  }
}
```
