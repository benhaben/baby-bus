using BabyBus.Data.Infrastructure;
using BabyBus.Model.Entities.Main;

namespace BabyBus.Data.Repositories.Main
{
    public class KindergartenRepository : RepositoryBase<Kindergarten>, IKindergartenRepository
    {
        public KindergartenRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
    }
    public interface IKindergartenRepository : IRepository<Kindergarten>
    {
    }
}
