using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Solution.Application;
using Solution.Application.UseCases.CreateGroup;
using Solution.Domain;
using Solution.Domain.Interfaces;
using Solution.Domain.Interfaces.Repositories;
using Solution.Tests.Fakes;
using System.Threading.Tasks;
using Xunit;

namespace Solution.Tests.UseCases
{
    public class CreateGroupUseCaseTests
    {
        private readonly Mock<IGroupRepository> _mockRepository;
        private readonly Mock<ILogger<CreateGroupUseCase>> _mockLogger;
        private readonly INotificationContext _notification;
        private readonly CreateGroupUseCase _useCase;

        public CreateGroupUseCaseTests()
        {
            _mockRepository = new Mock<IGroupRepository>();
            _mockLogger = new Mock<ILogger<CreateGroupUseCase>>();
            _notification = new NotificationContext();

            _useCase = new CreateGroupUseCase(
                _mockRepository.Object,
                _notification,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task Create_group_if_not_exists()
        {
            var input = new CreateGroupInput(new GroupId(1), "Test Group", "TestCategory");

            _mockRepository.Setup(x => x.Get(input.Id)).Returns((Group)null);

            var result = await _useCase.Execute(input);

            result.Success.Should().BeTrue();
            _mockRepository.Verify(x => x.Add(It.IsAny<Group>()), Times.Once);
            _mockRepository.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task Create_an_existing_group_is_invalid()
        {
            var input = new CreateGroupInput(new GroupId(1), "Test Group", "TestCategory");
            var existingGroup = GroupFaker.Generate();

            _mockRepository.Setup(x => x.Get(input.Id)).Returns(existingGroup);

            var result = await _useCase.Execute(input);

            result.Success.Should().BeFalse();
            _mockRepository.Verify(x => x.Add(It.IsAny<Group>()), Times.Never);
            _mockRepository.Verify(x => x.SaveAsync(), Times.Never);
        }
    }
}