using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Solution.Domain.Interfaces.Repositories
{
    public interface IGroupRepository
    {
        ImmutableArray<Group> GetAll(params GroupId[] ids);
        Group Get(GroupId id);
        void Add(Group group);
        void Update(Group group);
        Task SaveAsync();
    }
}
