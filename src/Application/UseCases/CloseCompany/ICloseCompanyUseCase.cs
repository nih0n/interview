using System.Threading.Tasks;

namespace Solution.Application.UseCases.CloseCompany
{
    public interface ICloseCompanyUseCase : IUseCase<CloseCompanyInput, Task<Result>> { }
}
