using Microsoft.Extensions.Logging;
using Solution.Domain;
using Solution.Domain.Interfaces;
using Solution.Domain.Interfaces.Repositories;
using System;
using System.Threading.Tasks;

namespace Solution.Application.UseCases.AddCompanyCost
{
    public class AddCompanyCostUseCase : UseCase<AddCompanyCostInput, Task<Result>>, IAddCompanyCostUseCase
    {
        private readonly ICompanyRepository _repository;
        private readonly ILogger<AddCompanyCostUseCase> _logger;

        public AddCompanyCostUseCase(
            ICompanyRepository repository,
            INotificationContext notification,
            ILogger<AddCompanyCostUseCase> logger) : base(notification)
            => (_repository, _logger) = (repository, logger);

        public override async Task<Result> Execute(AddCompanyCostInput input)
        {
            _logger.LogInformation("Getting company '{id}'", input.Id);

            var company = _repository.Get(input.Id);
            
            if (company is not null)
            {
                _logger.LogInformation("Adding cost '{type}' to company '{id}'", input.Cost.Type, input.Id);

                Cost cost;

                cost = company.Costs.Find(cost => cost.Type == input.Cost.Type);

                if (cost is not null)
                {
                    _logger.LogInformation("Increasing cost '{type}' value", cost.Type);

                    cost.Value += input.Cost.Value;
                }
                else
                {
                    _logger.LogInformation("Cost type not found, creating a new entry");

                    cost = new Cost(input.Cost.Year, input.Cost.Type, input.Cost.Value);
                }
                
                company.AddCost(cost);
                
                company.LastUpdate = DateTime.Now;

                _repository.Update(company);

                await _repository.SaveAsync();

                return Result.Ok();
            }

            _logger.LogError("Company '{id}' not found", input.Id);

            _notification.Add(Notifications.CompanyNotFound);

            return Result.Error(_notification.Notifications);
        }
    }
}
