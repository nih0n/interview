using Microsoft.Extensions.Logging;
using Moq;
using Solution.Application.UseCases.GetCompany;
using Solution.Domain;
using Solution.Domain.Interfaces;
using Solution.Domain.Interfaces.Repositories;
using Solution.Tests.Fakes;
using Xunit;

namespace Solution.Tests.UseCases
{
    public class GetCompanyUseCaseTests
    {
        private readonly Mock<ICompanyRepository> _repositoryMock;
        private readonly Mock<ILogger<GetCompanyUseCase>> _loggerMock;
        private readonly Mock<INotificationContext> _notificationContextMock;
        private readonly GetCompanyUseCase _useCase;

        public GetCompanyUseCaseTests()
        {
            _repositoryMock = new Mock<ICompanyRepository>();
            _loggerMock = new Mock<ILogger<GetCompanyUseCase>>();
            _notificationContextMock = new Mock<INotificationContext>();

            _useCase = new(
                _repositoryMock.Object,
                _notificationContextMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public void Get_company()
        {
            var input = new GetCompanyInput(new CompanyId("1A2"));

            var company = CompanyFaker.Generate();

            _repositoryMock.Setup(r => r.Get(input.Id))
                .Returns(company);

            var result = _useCase.Execute(input);

            Assert.True(result.Success);
            Assert.Equal(company, result.Data);
            _notificationContextMock.Verify(nc => nc.Add(It.IsAny<Notification>()), Times.Never);
        }

        [Fact]
        public void Not_found_company_with_specified_id()
        {
            var input = new GetCompanyInput(new("1A2"));

            _repositoryMock.Setup(r => r.Get(input.Id))
                .Returns((Company)null);

            var result = _useCase.Execute(input);

            Assert.False(result.Success);
        }
    }
}
