using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MediaLibrarian.Models.Utilities
{
    public class NullableUInt32Converter : JsonConverter<UInt32?>
    {
        public override UInt32? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            UInt32 number;
            try
            {
                return reader.GetUInt32();
            }
            catch
            {
                try
                {
                    if (UInt32.TryParse(reader.GetString(), out number))
                    {
                        return number;
                    }
                    return null;
                }
                catch
                {
                    return null;
                }
            }
        }

        public override void Write(Utf8JsonWriter writer, uint? value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

    }
}
