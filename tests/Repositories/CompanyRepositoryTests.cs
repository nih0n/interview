using FluentAssertions;
using Solution.Domain;
using Solution.Domain.Interfaces.Repositories;
using Solution.Infrastructure.Repositories;
using Solution.Tests.Fakes;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Solution.Tests.Repositories
{
    public class CompanyRepositoryTests
    {
        private readonly ICompanyRepository _repository;
        private readonly List<Company> _companies;

        public CompanyRepositoryTests()
        {
            _companies = CompanyFaker.Generate(3);

            var json = JsonSerializer.Serialize(_companies);

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

            _repository = new CompanyRepository(stream);
        }

        [Fact]
        public void Get_all_companies()
        {
            var result = _repository.GetAll();

            result.Should().HaveCount(_companies.Count);
        }

        [Fact]
        public void Get_companies_with_specific_ids()
        {
            var ids = new CompanyId[] {
                _companies[0].Id,
                _companies[1].Id
            };

            var result = _repository.GetAll(ids);

            result.Should().BeEquivalentTo(_companies.Where(c => ids.Contains(c.Id)));
        }

        [Fact]
        public void Get_company()
        {
            var company = _companies[0];

            var result = _repository.Get(company.Id);

            result.Id.Should().Be(company.Id);
        }

        [Fact]
        public void Add_company()
        {
            var company = CompanyFaker.Generate();

            _repository.Add(company);

            var companies = _repository.GetAll();

            companies.Last().Should().BeEquivalentTo(company);
        }

        [Fact]
        public void Update_company()
        {
            var company = _companies.First();

            company.Status = Status.Inactive;

            _repository.Update(company);

            var result = _repository.Get(company.Id);

            result.Should().BeEquivalentTo(company);
        }
    }
}
