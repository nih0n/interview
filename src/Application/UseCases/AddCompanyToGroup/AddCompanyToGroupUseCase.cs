using Microsoft.Extensions.Logging;
using Solution.Domain;
using Solution.Domain.Interfaces;
using Solution.Domain.Interfaces.Repositories;
using System.Threading.Tasks;

namespace Solution.Application.UseCases.AddCompanyToGroup
{
    public class AddCompanyToGroupUseCase : UseCase<AddCompanyToGroupInput, Task<Result>>, IAddCompanyToGroupUseCase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ILogger<AddCompanyToGroupUseCase> _logger;

        public AddCompanyToGroupUseCase(
            ICompanyRepository companyRepository,
            IGroupRepository groupRepository,
            INotificationContext notification,
            ILogger<AddCompanyToGroupUseCase> logger) : base(notification)
            => (_companyRepository, _groupRepository, _logger)
            = (companyRepository, groupRepository, logger);

        public override async Task<Result> Execute(AddCompanyToGroupInput input)
        {
            _logger.LogInformation("Getting group '{id}'", input.GroupId);

            var group = _groupRepository.Get(input.GroupId);

            if (group is not null)
            {
                _logger.LogInformation("Getting company '{id}'", input.CompanyId);

                var company = _companyRepository.Get(input.CompanyId);

                if (company is not null)
                {
                    _logger.LogInformation("Adding company '{companyId}' to group '{groupId}'", company.Id, group.Id);

                    group.AddCompany(company.Id);

                    _groupRepository.Update(group);

                    await _groupRepository.SaveAsync();

                    return Result.Ok();
                }

                _logger.LogError("Company '{id}' not found", input.CompanyId);

                _notification.Add(Notifications.CompanyAlreadyExists);

                return Result.Error(_notification.Notifications);
            }
            
            _logger.LogError("Group '{id}' not found", input.GroupId);

            _notification.Add(Notifications.GroupNotFound);

            return Result.Error(_notification.Notifications);
        }
    }
}
