using System;
#if NET8_0_OR_GREATER
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NOrganizze
{
    internal class DateOnlyJsonConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
        }
    }

    internal class NullableDateOnlyJsonConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var stringValue = reader.GetString();
            return string.IsNullOrEmpty(stringValue) ? (DateTime?)null : DateTime.Parse(stringValue);
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
                writer.WriteStringValue(value.Value.ToString("yyyy-MM-dd"));
            else
                writer.WriteNullValue();
        }
    }
}
#else
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NOrganizze
{
    internal class DateOnlyJsonConverter : IsoDateTimeConverter
    {
        public DateOnlyJsonConverter()
        {
            DateTimeFormat = "yyyy-MM-dd";
        }
    }
}
#endif
