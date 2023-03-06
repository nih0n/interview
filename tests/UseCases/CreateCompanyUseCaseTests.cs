using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Solution.Application;
using Solution.Application.UseCases.CreateCompany;
using Solution.Domain;
using Solution.Domain.Interfaces;
using Solution.Domain.Interfaces.Repositories;
using System.Threading.Tasks;
using Xunit;

namespace Solution.Tests.UseCases
{
    public class CreateCompanyUseCaseTests
    {
        private readonly Mock<ICompanyRepository> _repositoryMock;
        private readonly Mock<ILogger<CreateCompanyUseCase>> _loggerMock;
        private readonly INotificationContext _notification;
        private readonly CreateCompanyUseCase _useCase;

        public CreateCompanyUseCaseTests()
        {
            _repositoryMock = new Mock<ICompanyRepository>();
            _loggerMock = new Mock<ILogger<CreateCompanyUseCase>>();
            _notification = new NotificationContext();

            _useCase = new CreateCompanyUseCase(
                _repositoryMock.Object,
                _notification,
                _loggerMock.Object);
        }

        [Fact]
        public async Task Create_company()
        {
            var input = new CreateCompanyInput("1A2", Status.Active);

            _repositoryMock.Setup(x => x.Get(input.Id)).Returns((Company)null);

            var result = await _useCase.Execute(input);

            result.Should().BeEquivalentTo(Result.Ok());

            _repositoryMock.Verify(x => x.Add(It.IsAny<Company>()), Times.Once);
            _repositoryMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task Create_an_existing_company_is_invalid()
        {
            var input = new CreateCompanyInput("1A2", Status.Active);

            _repositoryMock.Setup(x => x.Get(input.Id)).Returns(new Company(input.Id, Status.Inactive));

            var result = await _useCase.Execute(input);

            _repositoryMock.Verify(x => x.Add(It.IsAny<Company>()), Times.Never);
            _repositoryMock.Verify(x => x.SaveAsync(), Times.Never);
        }
    }
}
