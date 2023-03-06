using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Solution.Application.UseCases.GetGroupCosts;
using Solution.Domain;
using Solution.Domain.Interfaces;
using Solution.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Xunit;

namespace Solution.Tests.UseCases
{
    public class GetGroupCostsUseCaseTests
    {
        private readonly Mock<IGroupRepository> _groupRepositoryMock;
        private readonly Mock<ICompanyRepository> _companyRepositoryMock;
        private readonly Mock<ILogger<GetGroupCostsUseCase>> _loggerMock;
        private readonly Mock<INotificationContext> _notificationContextMock;
        private readonly GetGroupCostsUseCase _useCase;

        public GetGroupCostsUseCaseTests()
        {
            _groupRepositoryMock = new Mock<IGroupRepository>();
            _companyRepositoryMock = new Mock<ICompanyRepository>();
            _loggerMock = new Mock<ILogger<GetGroupCostsUseCase>>();
            _notificationContextMock = new Mock<INotificationContext>();

            _useCase = new GetGroupCostsUseCase(
                _companyRepositoryMock.Object,
                _groupRepositoryMock.Object,
                _notificationContextMock.Object,
                _loggerMock.Object);
        }


        [Fact]
        public void Sum_companies_costs_grouped_by_type()
        {
            var input = new GetGroupCostsInput(1);

            var ids = new List<CompanyId> { "1A2", "1A3" };

            var group = new Group(input.Id.Value, "Test", "Test")
            {
                Companies = ids
            };
            
            var company1 = new Company("1A2", Status.Active);

            company1.AddCost(new Cost(2021, "TypeA", 100));
            company1.AddCost(new Cost(2022, "TypeB", 200));

            var company2 = new Company("1A3", Status.Active);

            company2.AddCost(new Cost(2021, "TypeA", 50));

            var companies = new List<Company> { company1, company2 };
            var expected = new Dictionary<string, Dictionary<uint, decimal>>
            {
                { "TypeA", new Dictionary<uint, decimal> { { 2021, 150 }, { 2022, 200 } } },
                { "TypeB", new Dictionary<uint, decimal> { { 2022, 200 } } }
            };

            _groupRepositoryMock
                .Setup(x => x.Get(input.Id))
                .Returns(group);

            _companyRepositoryMock
                .Setup(x => x.GetAll(group.Companies.ToArray()))
                .Returns(companies.ToImmutableArray());

            var result = _useCase.Execute(input);

            result.Success.Should().BeTrue();
            expected.First().Value.First().Value.Should().Be(150);
        }
    }
}
