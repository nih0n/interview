using System.Collections.Generic;

namespace Solution.Application.UseCases.GetGroupCosts
{
    public interface IGetGroupCostsUseCase : IUseCase<GetGroupCostsInput, Result<Dictionary<string, Dictionary<uint, decimal>>>> { }
}
