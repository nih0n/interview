using System.Threading.Tasks;

namespace Solution.Application.UseCases.CreateCompany
{
    public interface ICreateCompanyUseCase : IUseCase<CreateCompanyInput, Task<Result>> { }
}
