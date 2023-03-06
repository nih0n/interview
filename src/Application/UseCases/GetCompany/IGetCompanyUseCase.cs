using Solution.Domain;

namespace Solution.Application.UseCases.GetCompany
{
    public interface IGetCompanyUseCase : IUseCase<GetCompanyInput, Result<Company>> { }
}
