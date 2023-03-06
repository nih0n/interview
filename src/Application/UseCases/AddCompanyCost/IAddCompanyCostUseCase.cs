using System.Threading.Tasks;

namespace Solution.Application.UseCases.AddCompanyCost
{
    public interface IAddCompanyCostUseCase : IUseCase<AddCompanyCostInput, Task<Result>> { }
}
