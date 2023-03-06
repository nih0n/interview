using Solution.Domain;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Solution.Domain.Converters
{
    public class StatusJsonConverter : JsonConverter<Status>
    {
        public override Status Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string value = reader.GetString();

            return value switch
            {
                "ATIVO" => Status.Active,
                "INATIVO" => Status.Inactive,
                _ => throw new JsonException($"Unknown status value: {value}")
            };
        }

        public override void Write(Utf8JsonWriter writer, Status value, JsonSerializerOptions options)
            => writer.WriteStringValue(value == Status.Active ? "ATIVO" : "INATIVO");
    }

}
