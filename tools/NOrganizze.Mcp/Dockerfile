# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Directory.Build.props ./
COPY Directory.Packages.props ./
COPY src/NOrganizze/NOrganizze.csproj src/NOrganizze/
COPY tools/NOrganizze.Mcp/NOrganizze.Mcp.csproj tools/NOrganizze.Mcp/
RUN dotnet restore "tools/NOrganizze.Mcp/NOrganizze.Mcp.csproj"
COPY src/NOrganizze/ src/NOrganizze/
COPY tools/NOrganizze.Mcp/ tools/NOrganizze.Mcp/
RUN dotnet publish "tools/NOrganizze.Mcp/NOrganizze.Mcp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:10.0-alpine AS final
WORKDIR /app
RUN addgroup -S norganizze && adduser -S norganizze -G norganizze
COPY --from=build --chown=root:root --chmod=755 /app/publish .
USER norganizze

ENTRYPOINT ["dotnet", "NOrganizze.Mcp.dll"]
