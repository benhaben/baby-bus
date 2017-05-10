using BabyBus.Data.Infrastructure;
using BabyBus.Model.Entities.Relation;

namespace BabyBus.Data.Repositories.Relation
{
    public class ParentChildRelationRepository : RepositoryBase<ParentChildRelation>, IParentChildRelationRepository
    {
        public ParentChildRelationRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
    }
    public interface IParentChildRelationRepository : IRepository<ParentChildRelation>
    {
    }
}
