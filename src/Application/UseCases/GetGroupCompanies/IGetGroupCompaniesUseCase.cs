using Solution.Domain;
using System.Collections.Immutable;

namespace Solution.Application.UseCases.GetGroupCompanies
{
    public interface IGetGroupCompaniesUseCase : IUseCase<GetGroupCompaniesInput, Result<ImmutableArray<Company>>> { }
}
