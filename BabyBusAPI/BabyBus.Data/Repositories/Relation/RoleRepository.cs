using BabyBus.Data.Infrastructure;
using BabyBus.Model.Entities.Relation;

namespace BabyBus.Data.Repositories.Relation
{
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
    }
    public interface IRoleRepository : IRepository<Role>
    {
    }
}
