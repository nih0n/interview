using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Solution.Domain.Converters
{
    public class GroupIdJsonConverter : JsonConverter<GroupId>
    {
        public override GroupId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetUInt32());

        public override void Write(Utf8JsonWriter writer, GroupId value, JsonSerializerOptions options)
            => writer.WriteNumberValue(value.Value);
    }
}
