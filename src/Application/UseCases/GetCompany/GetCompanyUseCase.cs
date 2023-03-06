using Microsoft.Extensions.Logging;
using Solution.Domain;
using Solution.Domain.Interfaces;
using Solution.Domain.Interfaces.Repositories;

namespace Solution.Application.UseCases.GetCompany
{
    public class GetCompanyUseCase : UseCase<GetCompanyInput, Result<Company>>, IGetCompanyUseCase
    {
        private readonly ICompanyRepository _repository;
        private readonly ILogger<GetCompanyUseCase> _logger;

        public GetCompanyUseCase(
            ICompanyRepository repository,
            INotificationContext notification,
            ILogger<GetCompanyUseCase> logger) : base(notification)
            => (_repository, _logger) = (repository, logger);

        public override Result<Company> Execute(GetCompanyInput input)
        {
            _logger.LogInformation("Getting company '{id}'", input.Id.Value);

            var company = _repository.Get(input.Id);

            if (company is not null)
                return Result<Company>.Ok(company);

            _notification.Add(Notifications.CompanyNotFound);
            
            _logger.LogError("Company '{id}' not found", input.Id.Value);
            
            return Result<Company>.Error(_notification.Notifications);
        }
    }
}
