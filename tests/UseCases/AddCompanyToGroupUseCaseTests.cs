using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Solution.Application;
using Solution.Application.UseCases.AddCompanyToGroup;
using Solution.Domain;
using Solution.Domain.Interfaces.Repositories;
using Solution.Tests.Fakes;
using System.Threading.Tasks;
using Xunit;

namespace Solution.Tests.UseCases
{
    public class AddCompanyToGroupUseCaseTests
    {
        private readonly Mock<ICompanyRepository> _companyRepositoryMock;
        private readonly Mock<IGroupRepository> _groupRepositoryMock;
        private readonly Mock<ILogger<AddCompanyToGroupUseCase>> _loggerMock;
        private readonly AddCompanyToGroupUseCase _useCase;

        public AddCompanyToGroupUseCaseTests()
        {
            _companyRepositoryMock = new Mock<ICompanyRepository>();
            _groupRepositoryMock = new Mock<IGroupRepository>();
            _loggerMock = new Mock<ILogger<AddCompanyToGroupUseCase>>();
            
            _useCase = new(
                _companyRepositoryMock.Object,
                _groupRepositoryMock.Object,
                new NotificationContext(),
                _loggerMock.Object);
        }

        [Fact]
        public async Task Add_company_to_group()
        {
            var group = GroupFaker.Generate();
            var company = CompanyFaker.Generate();

            var input = new AddCompanyToGroupInput(group.Id, company.Id);

            _groupRepositoryMock.Setup(x => x.Get(input.GroupId)).Returns(group);
            _companyRepositoryMock.Setup(x => x.Get(input.CompanyId)).Returns(company);

            var result = await _useCase.Execute(input);

            result.Success.Should().BeTrue();

            _groupRepositoryMock.Verify(x => x.Update(It.Is<Group>(g => g.Companies.Contains(company.Id))), Times.Once);
            _groupRepositoryMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task Cannot_add_a_nonexistent_company_to_group()
        {
            var input = new AddCompanyToGroupInput(new(1), new("1A2"));

            _groupRepositoryMock.Setup(r => r.Get(input.GroupId)).Returns(GroupFaker.Generate());
            _companyRepositoryMock.Setup(r => r.Get(input.CompanyId)).Returns((Company)null);

            var result = await _useCase.Execute(input);

            result.Success.Should().BeFalse();
        }
    }
}
