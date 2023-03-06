using Microsoft.Extensions.Logging;
using Solution.Domain;
using Solution.Domain.Interfaces;
using Solution.Domain.Interfaces.Repositories;
using System.Collections.Generic;

namespace Solution.Application.UseCases.GetGroupCosts
{
    public class GetGroupCostsUseCase : UseCase<GetGroupCostsInput, Result<Dictionary<string, Dictionary<uint, decimal>>>>, IGetGroupCostsUseCase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ILogger<GetGroupCostsUseCase> _logger;

        public GetGroupCostsUseCase(
            ICompanyRepository companyRepository,
            IGroupRepository groupRepository,
            INotificationContext notification,
            ILogger<GetGroupCostsUseCase> logger) : base(notification)
            => (_companyRepository, _groupRepository, _logger)
            = (companyRepository, groupRepository, logger);

        public override Result<Dictionary<string, Dictionary<uint, decimal>>> Execute(GetGroupCostsInput input)
        {
            _logger.LogInformation("Getting company '{id}'", input.Id.Value);

            var group = _groupRepository.Get(input.Id);

            if (group is not null)
            {
                var companies = _companyRepository.GetAll(group.Companies.ToArray());

                _logger.LogInformation("Grouping costs");

                var costs = GroupCosts(companies);

                return Result<Dictionary<string, Dictionary<uint, decimal>>>.Ok(costs);
            }

            _notification.Add(Notifications.GroupNotFound);

            _logger.LogError("Company '{id}' not found", input.Id.Value);

            return Result<Dictionary<string, Dictionary<uint, decimal>>>.Error(_notification.Notifications);
        }

        private static Dictionary<string, Dictionary<uint, decimal>> GroupCosts(IEnumerable<Company> companies)
        {
            var output = new Dictionary<string, Dictionary<uint, decimal>>();

            foreach (var company in companies)
            {
                foreach (var cost in company.Costs)
                {
                    var type = cost.Type;
                    var year = cost.Year;
                    var value = cost.Value;

                    if (!output.ContainsKey(type))
                    {
                        output[type] = new Dictionary<uint, decimal>();
                    }

                    if (!output[type].ContainsKey(year))
                    {
                        output[type][year] = 0;
                    }

                    output[type][year] += value;
                }
            }

            return output;
        }
    }
}
