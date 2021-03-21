using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MediaLibrarian.Models.Utilities
{
    public class NullableDateTimeConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {

            int timestamp;
            try
            {
                if(reader.TryGetInt32(out timestamp))
                    return DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
            }
            catch
            {
                return null;
            }
            return null;
        }

        public override void Write(
            Utf8JsonWriter writer,
            DateTime? dateTimeValue,
            JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}