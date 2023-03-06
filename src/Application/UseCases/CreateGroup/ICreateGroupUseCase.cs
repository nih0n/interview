using System.Threading.Tasks;

namespace Solution.Application.UseCases.CreateGroup
{
    public interface ICreateGroupUseCase : IUseCase<CreateGroupInput, Task<Result>> { }
}
