using Solution.Domain.Converters;
using System;
using System.Text.Json.Serialization;

namespace Solution.Domain
{
    public class Cost
    {
        [JsonPropertyName("ano")]
        public uint Year { get; set; }

        [JsonPropertyName("id_type")]
        public string Type { get; set; }

        [JsonPropertyName("valor")]
        public decimal Value { get; set; }

        [JsonPropertyName("last_update")]
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime LastUpdate { get; set; }

        public Cost(uint year, string type, decimal value)
        {
            Value = value < 0 ? throw new ArgumentException("value cannot be less than 0", nameof(value)) : value;

            Year = year;
            Type = type;
            LastUpdate = DateTime.Now;
        }
    }
}
