using Microsoft.Extensions.Logging;
using Solution.Domain;
using Solution.Domain.Interfaces;
using Solution.Domain.Interfaces.Repositories;
using System;
using System.Threading.Tasks;

namespace Solution.Application.UseCases.CloseCompany
{
    public class CloseCompanyUseCase : UseCase<CloseCompanyInput, Task<Result>>, ICloseCompanyUseCase
    {
        private readonly ICompanyRepository _repository;
        private readonly ILogger<CloseCompanyUseCase> _logger;

        public CloseCompanyUseCase(
            ICompanyRepository repository,
            INotificationContext notification,
            ILogger<CloseCompanyUseCase> logger) : base(notification)
            => (_repository, _logger) = (repository, logger);

        public override async Task<Result> Execute(CloseCompanyInput input)
        {
            _logger.LogInformation("Getting company '{id}'", input.Id.Value);

            var company = _repository.Get(input.Id);
            
            if (company is not null)
            {
                _logger.LogInformation("Closing company '{id}'", input.Id.Value);

                company.Status = Status.Inactive;
                company.LastUpdate = DateTime.Now;
                
                _repository.Update(company);

                await _repository.SaveAsync();

                return Result.Ok();
            }

            _logger.LogError("Company '{id}' not found", input.Id.Value);

            _notification.Add(Notifications.CompanyAlreadyExists);

            return Result.Error(_notification.Notifications);
        }
    }
}
