using System;
using System.Globalization;
#if NET8_0_OR_GREATER
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NOrganizze
{
    internal class DateOnlyJsonConverter : JsonConverter<DateTime>
    {
        private const string DateFormat = "yyyy-MM-dd";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dateString = reader.GetString();
            return DateTime.ParseExact(dateString, DateFormat, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(DateFormat, CultureInfo.InvariantCulture));
        }
    }

    internal class NullableDateOnlyJsonConverter : JsonConverter<DateTime?>
    {
        private const string DateFormat = "yyyy-MM-dd";

        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var stringValue = reader.GetString();
            return string.IsNullOrEmpty(stringValue) 
                ? (DateTime?)null 
                : DateTime.ParseExact(stringValue, DateFormat, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
                writer.WriteStringValue(value.Value.ToString(DateFormat, CultureInfo.InvariantCulture));
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
            Culture = CultureInfo.InvariantCulture;
        }
    }
}
#endif
