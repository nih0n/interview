using Solution.Domain;

namespace Solution.Application.UseCases.CreateGroup
{
    public record CreateGroupInput(GroupId Id, string Name, string Category);
}
