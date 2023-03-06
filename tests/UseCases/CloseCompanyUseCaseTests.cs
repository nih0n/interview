using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Solution.Application;
using Solution.Application.UseCases.CloseCompany;
using Solution.Domain;
using Solution.Domain.Interfaces;
using Solution.Domain.Interfaces.Repositories;
using Solution.Tests.Fakes;
using System.Threading.Tasks;
using Xunit;

namespace Solution.Tests.UseCases
{
    public class CloseCompanyUseCaseTests
    {
        private readonly Mock<ICompanyRepository> _mockRepository;
        private readonly Mock<ILogger<CloseCompanyUseCase>> _mockLogger;
        private readonly INotificationContext _notification;
        private readonly CloseCompanyUseCase _useCase;

        public CloseCompanyUseCaseTests()
        {
            _mockRepository = new Mock<ICompanyRepository>();
            _mockLogger = new Mock<ILogger<CloseCompanyUseCase>>();
            _notification = new NotificationContext();

            _useCase = new(_mockRepository.Object, _notification, _mockLogger.Object);
        }

        [Fact]
        public async Task Closes_the_company()
        {
            var company = CompanyFaker.Generate();

            var input = new CloseCompanyInput(company.Id);
            
            _mockRepository.Setup(repo => repo.Get(company.Id)).Returns(company);
            
            var result = await _useCase.Execute(input);

            result.Success.Should().BeTrue();

            _mockRepository.Verify(repo => repo.Update(company));
            _mockRepository.Verify(repo => repo.SaveAsync());
        }

        [Fact]
        public async Task Cannot_close_a_nonexistent_company()
        {
            var input = new CloseCompanyInput(new("1A2"));

            _mockRepository.Setup(repo => repo.Get(input.Id)).Returns<Company>(null);

            var result = await _useCase.Execute(input);

            result.Success.Should().BeFalse();
        }
    }
}
