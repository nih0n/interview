using Microsoft.Extensions.Logging;
using Solution.Application.UseCases.GetCompany;
using Solution.Domain;
using Solution.Domain.Interfaces;
using Solution.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Solution.Application.UseCases.GetGroupCompanies
{
    public class GetGroupCompaniesUseCase : UseCase<GetGroupCompaniesInput, Result<ImmutableArray<Company>>>, IGetGroupCompaniesUseCase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ILogger<GetCompanyUseCase> _logger;

        public GetGroupCompaniesUseCase(
            ICompanyRepository companyRepository,
            IGroupRepository groupRepository,
            INotificationContext notification,
            ILogger<GetCompanyUseCase> logger) : base(notification)
            => (_companyRepository, _groupRepository, _logger)
            = (companyRepository, groupRepository, logger);

        public override Result<ImmutableArray<Company>> Execute(GetGroupCompaniesInput input)
        {
            _logger.LogInformation("Getting groups");

            var groups = _groupRepository.GetAll().Where(group => group.Ingestion.Date <= input.date.Date);

            if (groups.Any())
            {
                var companies = new List<Company>();
    
                foreach (var group in groups)
                {
                    _logger.LogInformation("Getting companies from group '{id}'", group.Id.Value);

                    companies.AddRange(_companyRepository.GetAll(group.Companies.ToArray()));
                }

                return Result<ImmutableArray<Company>>.Ok(companies.ToImmutableArray());
            }

            _logger.LogInformation("No groups satisfies the date filter");

            _notification.Add(Notifications.GroupNotFound);

            return Result<ImmutableArray<Company>>.Error(_notification.Notifications);
        }
    }
}
