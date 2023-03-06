using Solution.Domain;
using Solution.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;


namespace Solution.Infrastructure.Repositories
{
    public class GroupRepository : IGroupRepository, IDisposable
    {
        private readonly Stream _stream;
        private readonly List<Group> _cache;
        
        public GroupRepository(Stream stream)
        {
            _stream = stream;

            using var reader = new StreamReader(_stream, leaveOpen: true);

            _cache = JsonSerializer.Deserialize<List<Group>>(reader.ReadToEnd());
        }
        
        public ImmutableArray<Group> GetAll(params GroupId[] ids)
        {
            if (ids.Any())
                return _cache.FindAll(group => ids.Contains(group.Id)).ToImmutableArray();
            
            return _cache.ToImmutableArray();
        }

        public Group Get(GroupId id) => _cache.Find(group => group.Id == id);

        public void Add(Group group)
        {
            group.Ingestion = DateTime.Now;
            
            _cache.Add(group);
        }
        
        public void Update(Group group)
        {
            var index = _cache.FindIndex(entity => entity.Id == group.Id);
            
            _cache[index] = group;
        }

        public async Task SaveAsync()
        {
            _stream.SetLength(0);
            
            await JsonSerializer.SerializeAsync(_stream, _cache);

            await _stream.FlushAsync();
        }

        public void Dispose()
        {
            _stream.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
