using Bogus;
using Solution.Domain;
using System.Collections.Generic;

namespace Solution.Tests.Fakes
{
    public static class CostFaker
    {
        private static readonly Faker<Cost> _faker = new Faker<Cost>()
            .CustomInstantiator(faker => new Cost(
                (uint)faker.Date.Past().Year,
                faker.Random.String(),
                faker.Finance.Amount(min: 1)));

        public static Cost Generate() => _faker.Generate();

        public static List<Cost> Generate(int count) => _faker.Generate(count);
    }
}
