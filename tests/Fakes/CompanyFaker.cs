using Bogus;
using Solution.Domain;
using System.Collections.Generic;

namespace Solution.Tests.Fakes
{
    public static class CompanyFaker
    {
        private static readonly Faker<Company> _faker = new Faker<Company>()
            .CustomInstantiator(faker => new Company(faker.Random.String(3), faker.PickRandom<Status>()));

        public static Company Generate() => _faker.Generate();

        public static List<Company> Generate(int count) => _faker.Generate(count);
    }
}
