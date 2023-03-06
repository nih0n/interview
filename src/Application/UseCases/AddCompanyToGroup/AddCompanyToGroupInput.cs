using Solution.Domain;

namespace Solution.Application.UseCases.AddCompanyToGroup
{
    public record AddCompanyToGroupInput(GroupId GroupId, CompanyId CompanyId);
}
