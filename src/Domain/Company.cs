using Solution.Domain.Converters;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Solution.Domain
{
    public class Company
    {
        [JsonPropertyName("id")]
        [JsonConverter(typeof(CompanyIdJsonConverter))]
        public CompanyId Id { get; init; }

        [JsonPropertyName("status")]
        [JsonConverter(typeof(StatusJsonConverter))]
        public Status Status { get; set; }

        [JsonPropertyName("custos")]
        public List<Cost> Costs { get; set; }

        [JsonPropertyName("date_ingestion")]
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime Ingestion { get; set; }

        [JsonPropertyName("last_update")]
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime LastUpdate { get; set; }

        public Company(CompanyId id, Status status)
            => (Id, Status, Costs) = (id, status, new());

        public void AddCost(Cost cost) => Costs.Add(cost);
    }
}
