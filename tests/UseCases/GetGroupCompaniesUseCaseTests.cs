using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Solution.Application.UseCases.GetCompany;
using Solution.Application.UseCases.GetGroupCompanies;
using Solution.Domain;
using Solution.Domain.Interfaces;
using Solution.Domain.Interfaces.Repositories;
using Solution.Tests.Fakes;
using System;
using System.Collections.Immutable;
using System.Linq;
using Xunit;

namespace Solution.Tests.UseCases
{
    public class GetGroupCompaniesUseCaseTests
    {
        private readonly Mock<ICompanyRepository> _companyRepositoryMock;
        private readonly Mock<IGroupRepository> _groupRepositoryMock;
        private readonly Mock<ILogger<GetCompanyUseCase>> _loggerMock;
        private readonly Mock<INotificationContext> _notificationMock;
        private readonly GetGroupCompaniesUseCase _useCase;

        public GetGroupCompaniesUseCaseTests()
        {
            _companyRepositoryMock = new Mock<ICompanyRepository>();
            _groupRepositoryMock = new Mock<IGroupRepository>();
            _loggerMock = new Mock<ILogger<GetCompanyUseCase>>();
            _notificationMock = new Mock<INotificationContext>();
            _useCase = new GetGroupCompaniesUseCase(_companyRepositoryMock.Object, _groupRepositoryMock.Object, _notificationMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void No_groups_match_the_date_filter()
        {
            var input = new GetGroupCompaniesInput(DateTime.Now);

            _groupRepositoryMock.Setup(r => r.GetAll()).Returns(ImmutableArray<Group>.Empty);

            var result = _useCase.Execute(input);

            result.Success.Should().BeFalse();
        }

        [Fact]
        public void Get_groups_when_date_filter_match()
        {
            var input = new GetGroupCompaniesInput(DateTime.Now);

            var groups = GroupFaker.Generate(2);
            var companies = CompanyFaker.Generate(4);

            _groupRepositoryMock.Setup(r => r.GetAll()).Returns(groups.ToImmutableArray());
            
            _companyRepositoryMock
                .Setup(r => r.GetAll(It.IsAny<CompanyId[]>()))
                .Returns(companies.Where(c => groups.SelectMany(g => g.Companies).Contains(c.Id)).ToImmutableArray());

            var result = _useCase.Execute(input);

            result.Success.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(companies.Where(c => groups.SelectMany(g => g.Companies).Contains(c.Id)).ToImmutableArray());
            
            _groupRepositoryMock.Verify(r => r.GetAll(), Times.Once);
            _companyRepositoryMock.Verify(r => r.GetAll(It.IsAny<CompanyId[]>()));
        }
    }
}
