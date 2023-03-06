using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Solution.Application.UseCases.GetGroup;
using Solution.Domain;
using Solution.Domain.Interfaces;
using Solution.Domain.Interfaces.Repositories;
using Solution.Tests.Fakes;
using Xunit;

namespace Solution.Tests.UseCases
{
    public class GetGroupUseCaseTests
    {
        private readonly Mock<IGroupRepository> _groupRepositoryMock;
        private readonly Mock<INotificationContext> _notificationContextMock;
        private readonly Mock<ILogger<GetGroupUseCase>> _loggerMock;
        private readonly GetGroupUseCase _useCase;

        public GetGroupUseCaseTests()
        {
            _groupRepositoryMock = new Mock<IGroupRepository>();
            _notificationContextMock = new Mock<INotificationContext>();
            _loggerMock = new Mock<ILogger<GetGroupUseCase>>();

            _useCase = new GetGroupUseCase(_groupRepositoryMock.Object, _notificationContextMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void Get_group()
        {
            var input = new GetGroupInput(new GroupId(1));
            var group = GroupFaker.Generate();

            _groupRepositoryMock.Setup(r => r.Get(input.Id)).Returns(group);

            var result = _useCase.Execute(input);

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(group);
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Not_found_group_with_specified_id()
        {
            var input = new GetGroupInput(new GroupId(1));

            _groupRepositoryMock.Setup(r => r.Get(input.Id)).Returns<Group>(null);

            var result = _useCase.Execute(input);

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Data.Should().BeNull();
        }
    }
}
