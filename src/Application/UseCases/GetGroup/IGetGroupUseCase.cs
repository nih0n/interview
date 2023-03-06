using Solution.Domain;

namespace Solution.Application.UseCases.GetGroup
{
    public interface IGetGroupUseCase : IUseCase<GetGroupInput, Result<Group>> { }
}
