using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Solution.Domain.Converters
{
    public class CompanyIdJsonConverter : JsonConverter<CompanyId>
    {
        public override CompanyId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => new(reader.GetString());

        public override void Write(Utf8JsonWriter writer, CompanyId value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.Value);
    }
}
