using System.Text.Json.Serialization;

namespace Solution.API.Requests
{
    public record AddCompanyCostRequest(
        [property: JsonPropertyName("ano")] uint Year,
        [property: JsonPropertyName("id_type")] string Type,
        [property: JsonPropertyName("valor")] decimal Value);
}
