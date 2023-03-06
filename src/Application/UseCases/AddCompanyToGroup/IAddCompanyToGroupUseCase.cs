using System.Threading.Tasks;

namespace Solution.Application.UseCases.AddCompanyToGroup
{
    public interface IAddCompanyToGroupUseCase : IUseCase<AddCompanyToGroupInput, Task<Result>> { }
}
