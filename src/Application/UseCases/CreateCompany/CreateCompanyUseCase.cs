using Microsoft.Extensions.Logging;
using Solution.Domain;
using Solution.Domain.Interfaces;
using Solution.Domain.Interfaces.Repositories;
using System.Threading.Tasks;

namespace Solution.Application.UseCases.CreateCompany
{
    public class CreateCompanyUseCase : UseCase<CreateCompanyInput, Task<Result>>, ICreateCompanyUseCase
    {
        private readonly ICompanyRepository _repository;
        private readonly ILogger<CreateCompanyUseCase> _logger;

        public CreateCompanyUseCase(
            ICompanyRepository repository,
            INotificationContext notification,
            ILogger<CreateCompanyUseCase> logger) : base(notification)
            => (_repository, _logger) = (repository, logger);

        public override async Task<Result> Execute(CreateCompanyInput input)
        {
            _logger.LogInformation("Checking if company '{id}' exists", input.Id);

            var entity = _repository.Get(input.Id);

            if (entity is null)
            {
                _logger.LogInformation("Creating company '{id}'", input.Id);

                var company = new Company(input.Id, input.Status);

                _repository.Add(company);

                await _repository.SaveAsync();

                return Result.Ok();
            }

            _logger.LogError("Company '{id}' already exists", input.Id);

            _notification.Add(Notifications.CompanyAlreadyExists);

            return Result.Error(_notification.Notifications);
        }
    }
}