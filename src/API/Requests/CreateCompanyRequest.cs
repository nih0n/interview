using Solution.Domain;
using Solution.Domain.Converters;
using System.Text.Json.Serialization;

namespace Solution.API.Requests
{
    public record CreateCompanyRequest(
        [property: JsonPropertyName("id")]
        string Id,
        [property: JsonPropertyName("status")]
        [property: JsonConverter(typeof(StatusJsonConverter))]
        Status Status);
}
