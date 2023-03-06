using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Solution.Application;
using Solution.Application.UseCases.AddCompanyCost;
using Solution.Domain;
using Solution.Domain.Interfaces;
using Solution.Domain.Interfaces.Repositories;
using Solution.Tests.Fakes;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Solution.Tests.UseCases
{
    public class AddCompanyCostUseCaseTests
    {
        private readonly Mock<ICompanyRepository> _repositoryMock;
        private readonly Mock<ILogger<AddCompanyCostUseCase>> _loggerMock;
        private readonly INotificationContext _notification;
        private readonly AddCompanyCostUseCase _useCase;

        public AddCompanyCostUseCaseTests()
        {
            _repositoryMock = new Mock<ICompanyRepository>();
            _loggerMock = new Mock<ILogger<AddCompanyCostUseCase>>();
            _notification = new NotificationContext();

            _useCase = new AddCompanyCostUseCase(
                _repositoryMock.Object,
                _notification,
                _loggerMock.Object);
        }

        [Fact]
        public async Task Cannot_add_cost_to_a_nonexistent_company()
        {
            var input = new AddCompanyCostInput(new CompanyId("1A2"), new CreateCostInput(2022, "Test", 1000.00m));

            _repositoryMock.Setup(x => x.Get(input.Id)).Returns((Company)null);

            var result = await _useCase.Execute(input);

            result.Success.Should().BeFalse();
        }

        [Fact]
        public async Task Increase_cost_value_when_adding_cost_of_same_type()
        {
            var company = CompanyFaker.Generate();
            var cost = new Cost(2022, "Test", 100);

            company.AddCost(cost);
            
            var input = new AddCompanyCostInput(company.Id, new CreateCostInput(2022, cost.Type, 100));
            _repositoryMock.Setup(x => x.Get(input.Id)).Returns(company);

            var result = await _useCase.Execute(input);

            result.Success.Should().BeTrue();
            company.Costs.First().Value.Should().Be(200m);
        }
    }
}