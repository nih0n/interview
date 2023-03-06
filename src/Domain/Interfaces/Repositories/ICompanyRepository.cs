using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Solution.Domain.Interfaces.Repositories
{
    public interface ICompanyRepository
    {
        ImmutableArray<Company> GetAll(params CompanyId[] ids);
        Company Get(CompanyId id);
        void Add(Company company);
        void Update(Company company);
        Task SaveAsync();
    }
}
