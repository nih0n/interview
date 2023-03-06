using Bogus;
using Solution.Domain;
using System.Collections.Generic;

namespace Solution.Tests.Fakes
{
    public static class GroupFaker
    {
        private static readonly Faker<Group> _faker = new Faker<Group>()
            .CustomInstantiator(faker => new Group(faker.Random.UInt(), faker.Random.String(2), faker.Random.String(1)));

        public static Group Generate() => _faker.Generate();

        public static List<Group> Generate(int count) => _faker.Generate(count);
    }
}
