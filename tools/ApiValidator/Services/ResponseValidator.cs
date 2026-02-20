using System.Reflection;
using System.Text.Json;
using ApiValidator.Models;
using Newtonsoft.Json.Linq;

namespace ApiValidator.Services;

public class ResponseValidator
{
    public ValidationResult ValidateResponse<T>(string jsonResponse, T deserializedObject)
    {
        var result = new ValidationResult { Passed = true };

        try
        {
            // Parse the JSON to inspect actual structure
            var jToken = JToken.Parse(jsonResponse);

            // Get the type we're validating against
            var targetType = typeof(T);

            // Handle collections
            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(List<>))
            {
                var elementType = targetType.GetGenericArguments()[0];
                if (jToken is JArray jArray && jArray.Count > 0)
                {
                    // Validate first element as a sample
                    ValidateObject(jArray[0], elementType, result, "");
                }
            }
            else
            {
                ValidateObject(jToken, targetType, result, "");
            }
        }
        catch (Exception ex)
        {
            result.Passed = false;
            result.Errors.Add($"Validation error: {ex.Message}");
        }

        return result;
    }

    private void ValidateObject(JToken jToken, Type targetType, ValidationResult result, string path)
    {
        if (jToken is not JObject jObject)
            return;

        var properties = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var jsonPropertyNames = jObject.Properties().Select(p => p.Name).ToHashSet();

        // Check for properties in C# model
        foreach (var prop in properties)
        {
            var jsonPropertyName = GetJsonPropertyName(prop);
            var fullPath = string.IsNullOrEmpty(path) ? jsonPropertyName : $"{path}.{jsonPropertyName}";

            if (!jsonPropertyNames.Contains(jsonPropertyName))
            {
                // Property exists in C# but not in JSON
                // Only flag it if it's not nullable
                if (!IsNullableType(prop.PropertyType))
                {
                    result.Discrepancies.Add(new PropertyDiscrepancy
                    {
                        PropertyName = fullPath,
                        Issue = "Property exists in C# model but missing in JSON response",
                        ExpectedType = prop.PropertyType.Name,
                        ActualType = "missing"
                    });
                    result.Passed = false;
                }
            }
            else
            {
                // Validate type compatibility
                var jsonValue = jObject[jsonPropertyName];
                if (jsonValue != null && jsonValue.Type != JTokenType.Null)
                {
                    ValidateTypeCompatibility(jsonValue, prop.PropertyType, fullPath, result);
                }
            }
        }

        // Check for extra properties in JSON not in C# model
        foreach (var jsonProp in jObject.Properties())
        {
            var matchingProperty = properties.FirstOrDefault(p =>
                GetJsonPropertyName(p).Equals(jsonProp.Name, StringComparison.OrdinalIgnoreCase));

            if (matchingProperty == null)
            {
                var fullPath = string.IsNullOrEmpty(path) ? jsonProp.Name : $"{path}.{jsonProp.Name}";
                result.Discrepancies.Add(new PropertyDiscrepancy
                {
                    PropertyName = fullPath,
                    Issue = "Property exists in JSON response but not in C# model",
                    ExpectedType = "not defined",
                    ActualType = jsonProp.Value?.Type.ToString() ?? "unknown"
                });
                // Don't fail validation for extra properties, just note them
            }
        }
    }

    private void ValidateTypeCompatibility(JToken jsonValue, Type propertyType, string propertyName, ValidationResult result)
    {
        var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

        bool isCompatible = underlyingType.Name switch
        {
            nameof(String) => jsonValue.Type == JTokenType.String,
            nameof(Int32) or nameof(Int64) => jsonValue.Type == JTokenType.Integer,
            nameof(Decimal) or nameof(Double) or nameof(Single) =>
                jsonValue.Type == JTokenType.Float || jsonValue.Type == JTokenType.Integer,
            nameof(Boolean) => jsonValue.Type == JTokenType.Boolean,
            nameof(DateTime) or "DateOnly" => jsonValue.Type == JTokenType.Date || jsonValue.Type == JTokenType.String,
            _ => true // For complex types, don't validate strictly
        };

        if (!isCompatible)
        {
            result.Discrepancies.Add(new PropertyDiscrepancy
            {
                PropertyName = propertyName,
                Issue = "Type mismatch",
                ExpectedType = underlyingType.Name,
                ActualType = jsonValue.Type.ToString()
            });
            result.Passed = false;
        }

        // Recursively validate complex types
        if (jsonValue.Type == JTokenType.Object && underlyingType.IsClass && underlyingType != typeof(string))
        {
            ValidateObject(jsonValue, underlyingType, result, propertyName);
        }
        else if (jsonValue.Type == JTokenType.Array && underlyingType.IsGenericType)
        {
            var elementType = underlyingType.GetGenericArguments()[0];
            var jArray = (JArray)jsonValue;
            if (jArray.Count > 0)
            {
                ValidateObject(jArray[0], elementType, result, $"{propertyName}[0]");
            }
        }
    }

    private string GetJsonPropertyName(PropertyInfo prop)
    {
        // Check for JsonPropertyName attribute first
        var jsonAttr = prop.GetCustomAttribute<System.Text.Json.Serialization.JsonPropertyNameAttribute>();
        if (jsonAttr != null)
            return jsonAttr.Name;

        // Check for Newtonsoft JsonProperty attribute
        var newtonsoftAttr = prop.GetCustomAttribute<Newtonsoft.Json.JsonPropertyAttribute>();
        if (newtonsoftAttr?.PropertyName != null)
            return newtonsoftAttr.PropertyName;

        // Default to camelCase
        return ToCamelCase(prop.Name);
    }

    private string ToCamelCase(string str)
    {
        if (string.IsNullOrEmpty(str) || char.IsLower(str[0]))
            return str;
        return char.ToLower(str[0]) + str.Substring(1);
    }

    private bool IsNullableType(Type type)
    {
        return !type.IsValueType || Nullable.GetUnderlyingType(type) != null;
    }
}
