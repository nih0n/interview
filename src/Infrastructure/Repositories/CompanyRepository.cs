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
    public class CompanyRepository : ICompanyRepository, IDisposable
    {
        private readonly Stream _stream;
        private readonly List<Company> _cache;

        public CompanyRepository(Stream stream)
        {
            _stream = stream;
            
            using var reader = new StreamReader(_stream, leaveOpen: true);

            _cache = JsonSerializer.Deserialize<List<Company>>(reader.ReadToEnd());
        }

        public ImmutableArray<Company> GetAll(params CompanyId[] ids)
        {
            if (ids.Any())
                return _cache.FindAll(company => ids.Contains(company.Id)).ToImmutableArray();
            
            return _cache.ToImmutableArray();
        }

        public Company Get(CompanyId id) => _cache.Find(company => company.Id == id);

        public void Add(Company company)
        {
            company.Ingestion = DateTime.Now;
            company.LastUpdate = DateTime.Now;

            _cache.Add(company);
        }

        public void Update(Company company)
        {
            var index = _cache.FindIndex(entity => entity.Id == company.Id);
            
            company.LastUpdate = DateTime.Now;

            _cache[index] = company;
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
