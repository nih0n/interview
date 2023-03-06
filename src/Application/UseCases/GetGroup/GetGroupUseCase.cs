using Microsoft.Extensions.Logging;
using Solution.Domain;
using Solution.Domain.Interfaces;
using Solution.Domain.Interfaces.Repositories;

namespace Solution.Application.UseCases.GetGroup
{
    public class GetGroupUseCase : UseCase<GetGroupInput, Result<Group>>, IGetGroupUseCase
    {
        private readonly IGroupRepository _repository;
        private readonly ILogger<GetGroupUseCase> _logger;

        public GetGroupUseCase(
            IGroupRepository repository,
            INotificationContext notification,
            ILogger<GetGroupUseCase> logger) : base(notification)
            => (_repository, _logger) = (repository, logger);

        public override Result<Group> Execute(GetGroupInput input)
        {
            _logger.LogInformation("Getting group '{id}'", input.Id.Value);

            var group = _repository.Get(input.Id);

            if (group is not null)
                return Result<Group>.Ok(group);

            _notification.Add(Notifications.GroupNotFound);

            _logger.LogError("Group '{id}' not found", input.Id.Value);

            return Result<Group>.Error(_notification.Notifications);
        }
    }
}
