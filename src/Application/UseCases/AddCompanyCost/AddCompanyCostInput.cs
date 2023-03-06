using Solution.Domain;

namespace Solution.Application.UseCases.AddCompanyCost
{
    public record AddCompanyCostInput(CompanyId Id, CreateCostInput Cost);
}
