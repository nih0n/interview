using Microsoft.Extensions.Logging;
using Solution.Domain;
using Solution.Domain.Interfaces;
using Solution.Domain.Interfaces.Repositories;
using System.Threading.Tasks;

namespace Solution.Application.UseCases.CreateGroup
{
    public class CreateGroupUseCase : UseCase<CreateGroupInput, Task<Result>>, ICreateGroupUseCase
    {
        private readonly IGroupRepository _repository;
        private readonly ILogger<CreateGroupUseCase> _logger;

        public CreateGroupUseCase(
            IGroupRepository repository,
            INotificationContext notification,
            ILogger<CreateGroupUseCase> logger) : base(notification)
            => (_repository, _logger) = (repository, logger);

        public override async Task<Result> Execute(CreateGroupInput input)
        {
            _logger.LogInformation("Checking if group '{id}' exists", input.Id.Value);

            var entity = _repository.Get(input.Id);
            
            if (entity is null)
            {
                _logger.LogInformation("Creating group '{id}'", input.Id.Value);

                var group = new Group(input.Id, input.Name, input.Category);
                
                _repository.Add(group);

                await _repository.SaveAsync();

                return Result.Ok();
            }

            _logger.LogError("Group '{id}' already exists", input.Id.Value);

            _notification.Add(Notifications.GroupAlreadyExists);

            return Result.Error(_notification.Notifications);
        }
    }
}
