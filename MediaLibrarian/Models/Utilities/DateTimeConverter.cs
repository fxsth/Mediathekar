using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MediaLibrarian.Models.Utilities
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) =>
                DateTimeOffset.FromUnixTimeSeconds(reader.GetInt32()).DateTime;

        public override void Write(
            Utf8JsonWriter writer,
            DateTime dateTimeValue,
            JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}