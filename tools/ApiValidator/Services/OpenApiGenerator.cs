using System.Text;
using System.Text.RegularExpressions;
using ApiValidator.Models;
using Newtonsoft.Json.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ApiValidator.Services;

public class OpenApiGenerator
{
    private readonly Dictionary<string, object> _openApiDoc = new();
    private readonly Dictionary<string, object> _paths = new();
    private readonly Dictionary<string, object> _schemas = new();

    public void GenerateFromResults(List<EndpointResult> results)
    {
        // Initialize OpenAPI document structure
        _openApiDoc["openapi"] = "3.0.3";
        _openApiDoc["info"] = new Dictionary<string, object>
        {
            { "title", "Organizze API" },
            { "version", "2.0.0" },
            { "description", "Empirically generated OpenAPI specification from actual API responses\n\nGenerated on: " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + " (UTC)" }
        };
        _openApiDoc["servers"] = new List<object>
        {
            new Dictionary<string, object>
            {
                { "url", "https://api.organizze.com.br/rest/v2" },
                { "description", "Production server" }
            }
        };
        _openApiDoc["security"] = new List<object>
        {
            new Dictionary<string, object>
            {
                { "basicAuth", new List<object>() }
            }
        };

        // Process each successful endpoint
        foreach (var result in results.Where(r => r.Success && !string.IsNullOrEmpty(r.RawResponse)))
        {
            AddEndpointToSpec(result);
        }

        _openApiDoc["paths"] = _paths;
        _openApiDoc["components"] = new Dictionary<string, object>
        {
            { "securitySchemes", new Dictionary<string, object>
                {
                    { "basicAuth", new Dictionary<string, object>
                        {
                            { "type", "http" },
                            { "scheme", "basic" },
                            { "description", "Email as username, API token as password" }
                        }
                    }
                }
            },
            { "schemas", _schemas }
        };
    }

    private void AddEndpointToSpec(EndpointResult result)
    {
        try
        {
            // Normalize path (remove IDs to make it a template)
            var pathTemplate = NormalizePathToTemplate(result.Path);

            if (!_paths.ContainsKey(pathTemplate))
            {
                _paths[pathTemplate] = new Dictionary<string, object>();
            }

            var pathItem = (Dictionary<string, object>)_paths[pathTemplate];
            var method = result.Method.ToLower();

            var operation = new Dictionary<string, object>
            {
                { "summary", GenerateSummary(result) },
                { "tags", new List<string> { result.Service } },
                { "responses", new Dictionary<string, object>() }
            };

            // Add path parameters
            var pathParams = ExtractPathParameters(result.Path);
            if (pathParams.Count > 0)
            {
                operation["parameters"] = pathParams;
            }

            // Add response
            var responses = (Dictionary<string, object>)operation["responses"];
            var statusCode = result.StatusCode.ToString();

            if (!string.IsNullOrEmpty(result.RawResponse))
            {
                var schemaName = GenerateSchemaFromResponse(result);
                responses[statusCode] = new Dictionary<string, object>
                {
                    { "description", "Successful response" },
                    { "content", new Dictionary<string, object>
                        {
                            { "application/json", new Dictionary<string, object>
                                {
                                    { "schema", new Dictionary<string, object>
                                        {
                                            { "$ref", $"#/components/schemas/{schemaName}" }
                                        }
                                    }
                                }
                            }
                        }
                    }
                };
            }

            // Add common error responses
            responses["401"] = new Dictionary<string, object>
            {
                { "description", "Unauthorized" }
            };

            pathItem[method] = operation;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Warning: Could not add endpoint {result.Path} to spec: {ex.Message}");
        }
    }

    private string NormalizePathToTemplate(string path)
    {
        // Replace numeric IDs with parameters
        var parts = path.Split('/');
        for (int i = 0; i < parts.Length; i++)
        {
            if (long.TryParse(parts[i], out _))
            {
                // Determine parameter name from context
                var paramName = i > 0 ? GetParameterNameFromContext(parts[i - 1]) : "id";
                parts[i] = $"{{{paramName}}}";
            }
        }
        return string.Join("/", parts);
    }

    private string GetParameterNameFromContext(string context)
    {
        return context switch
        {
            "accounts" => "accountId",
            "categories" => "categoryId",
            "credit_cards" => "creditCardId",
            "invoices" => "invoiceId",
            "transactions" => "transactionId",
            "transfers" => "transferId",
            "users" => "userId",
            "budgets" => "year", // Special case
            _ => "id"
        };
    }

    private List<object> ExtractPathParameters(string path)
    {
        var parameters = new List<object>();
        var templatePath = NormalizePathToTemplate(path);
        var matches = Regex.Matches(templatePath, @"\{([^}]+)\}", RegexOptions.None, TimeSpan.FromSeconds(1));

        foreach (Match match in matches)
        {
            var paramName = match.Groups[1].Value;
            var paramType = paramName == "year" || paramName == "month" ? "integer" : "integer";

            parameters.Add(new Dictionary<string, object>
            {
                { "name", paramName },
                { "in", "path" },
                { "required", true },
                { "schema", new Dictionary<string, object>
                    {
                        { "type", paramType },
                        { "format", paramType == "integer" ? "int64" : null }
                    }
                }
            });
        }

        return parameters;
    }

    private string GenerateSummary(EndpointResult result)
    {
        var action = result.Method switch
        {
            "GET" => result.Path.Contains("{") ? "Get" : "List",
            "POST" => "Create",
            "PUT" => "Update",
            "DELETE" => "Delete",
            _ => result.Method
        };

        return $"{action} {result.Service.TrimEnd('s')}";
    }

    private string GenerateSchemaFromResponse(EndpointResult result)
    {
        try
        {
            var json = result.RawResponse!;
            var jToken = JToken.Parse(json);

            string schemaName;

            if (jToken is JArray jArray)
            {
                schemaName = result.Service; // e.g., "Accounts", "Transactions"

                if (jArray.Count > 0)
                {
                    var itemSchema = GenerateSchemaFromJToken(jArray[0], schemaName.TrimEnd('s'));
                    var arraySchemaName = $"{schemaName}Array";

                    if (!_schemas.ContainsKey(arraySchemaName))
                    {
                        _schemas[arraySchemaName] = new Dictionary<string, object>
                        {
                            { "type", "array" },
                            { "items", new Dictionary<string, object>
                                {
                                    { "$ref", $"#/components/schemas/{schemaName.TrimEnd('s')}" }
                                }
                            }
                        };
                    }

                    return arraySchemaName;
                }
            }
            else
            {
                schemaName = result.Service.TrimEnd('s'); // e.g., "Account", "Transaction"
                GenerateSchemaFromJToken(jToken, schemaName);
            }

            return schemaName;
        }
        catch
        {
            return "Unknown";
        }
    }

    private Dictionary<string, object> GenerateSchemaFromJToken(JToken jToken, string schemaName)
    {
        if (_schemas.ContainsKey(schemaName) && jToken is JObject)
        {
            // Merge properties if schema already exists
            var existingSchema = (Dictionary<string, object>)_schemas[schemaName];
            var existingProps = (Dictionary<string, object>)existingSchema["properties"];
            var newProps = GeneratePropertiesFromJObject((JObject)jToken);

            foreach (var kvp in newProps)
            {
                if (!existingProps.ContainsKey(kvp.Key))
                {
                    existingProps[kvp.Key] = kvp.Value;
                }
            }

            return existingSchema;
        }

        var schema = new Dictionary<string, object>
        {
            { "type", "object" },
            { "properties", new Dictionary<string, object>() }
        };

        if (jToken is JObject jObject)
        {
            schema["properties"] = GeneratePropertiesFromJObject(jObject);
        }

        _schemas[schemaName] = schema;
        return schema;
    }

    private Dictionary<string, object> GeneratePropertiesFromJObject(JObject jObject)
    {
        var properties = new Dictionary<string, object>();

        foreach (var prop in jObject.Properties())
        {
            var propSchema = new Dictionary<string, object>();
            var value = prop.Value;

            switch (value.Type)
            {
                case JTokenType.Integer:
                    propSchema["type"] = "integer";
                    propSchema["format"] = "int64";
                    break;
                case JTokenType.Float:
                    propSchema["type"] = "number";
                    propSchema["format"] = "double";
                    break;
                case JTokenType.String:
                    propSchema["type"] = "string";
                    // Check if it looks like a date
                    if (DateTime.TryParse(value.ToString(), out _))
                    {
                        propSchema["format"] = "date-time";
                    }
                    break;
                case JTokenType.Boolean:
                    propSchema["type"] = "boolean";
                    break;
                case JTokenType.Array:
                    propSchema["type"] = "array";
                    if (value is JArray arr && arr.Count > 0)
                    {
                        var itemType = arr[0].Type;
                        propSchema["items"] = new Dictionary<string, object>
                        {
                            { "type", MapJTokenTypeToOpenApiType(itemType) }
                        };
                    }
                    break;
                case JTokenType.Object:
                    propSchema["type"] = "object";
                    break;
                case JTokenType.Null:
                    propSchema["nullable"] = true;
                    break;
            }

            properties[prop.Name] = propSchema;
        }

        return properties;
    }

    private string MapJTokenTypeToOpenApiType(JTokenType type)
    {
        return type switch
        {
            JTokenType.Integer => "integer",
            JTokenType.Float => "number",
            JTokenType.String => "string",
            JTokenType.Boolean => "boolean",
            JTokenType.Array => "array",
            JTokenType.Object => "object",
            _ => "string"
        };
    }

    public void SaveToYaml(string filePath)
    {
        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var yaml = serializer.Serialize(_openApiDoc);
        File.WriteAllText(filePath, yaml);
    }
}
