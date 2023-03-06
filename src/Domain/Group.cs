using Solution.Domain.Converters;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Solution.Domain
{
    public class Group
    {
        [JsonPropertyName("id")]
        [JsonConverter(typeof(GroupIdJsonConverter))]
        public GroupId Id { get; init; }

        [JsonPropertyName("nome")]
        public string Name { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("companys")]
        [JsonConverter(typeof(CompanyIdListJsonConverter))]
        public List<CompanyId> Companies { get; set; }
        
        [JsonPropertyName("date_ingestion")]
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime Ingestion { get; set; }

        public Group(GroupId id, string name, string category)
        {
            Name = string.IsNullOrEmpty(name) ? throw new ArgumentException("name cannot be empty", nameof(name)) : name;
            Category = string.IsNullOrEmpty(category) ? throw new ArgumentException("category cannot be empty", nameof(category)) : category;

            Id = id;
            Companies = new();
        }

        public void AddCompany(CompanyId id) => Companies.Add(id);
    }

    public class CompanyIdListJsonConverter : JsonConverter<List<CompanyId>>
    {
        public override List<CompanyId> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var ids = new List<CompanyId>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    return ids;
                }

                if (reader.TokenType != JsonTokenType.String)
                {
                    throw new JsonException($"Unexpected token type: {reader.TokenType}");
                }

                var value = reader.GetString();
                ids.Add(new CompanyId(value));
            }

            throw new JsonException("Unexpected end of JSON array.");
        }

        public override void Write(Utf8JsonWriter writer, List<CompanyId> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            foreach (var id in value)
            {
                writer.WriteStringValue(id.Value);
            }

            writer.WriteEndArray();
        }
    }
}
