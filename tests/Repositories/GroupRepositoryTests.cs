using FluentAssertions;
using Solution.Domain;
using Solution.Domain.Interfaces.Repositories;
using Solution.Infrastructure.Repositories;
using Solution.Tests.Fakes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Solution.Tests.Repositories
{
    public class GroupRepositoryTests
    {
        private readonly IGroupRepository _repository;
        private readonly List<Group> _groups;

        public GroupRepositoryTests()
        {
            _groups = GroupFaker.Generate(3);

            var json = JsonSerializer.Serialize(_groups);

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

            _repository = new GroupRepository(stream);
        }

        [Fact]
        public void Get_all_groups()
        {
            var result = _repository.GetAll();

            result.Should().HaveCount(_groups.Count);
        }

        [Fact]
        public void Get_groups_with_specific_ids()
        {
            var ids = new GroupId[] {
                _groups[0].Id,
                _groups[1].Id
            };

            var result = _repository.GetAll(ids);

            result.Should().BeEquivalentTo(_groups.Where(c => ids.Contains(c.Id)));
        }

        [Fact]
        public void Get_group()
        {
            var group = _groups[0];

            var result = _repository.Get(group.Id);

            result.Id.Should().Be(group.Id);
        }

        [Fact]
        public void Add_group()
        {
            var group = GroupFaker.Generate();

            _repository.Add(group);

            var groups = _repository.GetAll();

            groups.Last().Should().BeEquivalentTo(group);
        }

        [Fact]
        public void Update_group()
        {
            var group = _groups.First();

            group.Name = "Changed";

            _repository.Update(group);

            var result = _repository.Get(group.Id);

            result.Should().BeEquivalentTo(group);
        }
    }
}
