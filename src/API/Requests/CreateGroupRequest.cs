using Solution.Domain;
using System.Text.Json.Serialization;

namespace Solution.API.Requests
{
    public record CreateGroupRequest(
        [property: JsonPropertyName("id")] GroupId Id,
        [property: JsonPropertyName("nome")] string Name,
        [property: JsonPropertyName("category")] string category);
}
