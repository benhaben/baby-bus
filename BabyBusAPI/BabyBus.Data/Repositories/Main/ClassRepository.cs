using BabyBus.Data.Infrastructure;
using BabyBus.Model.Entities.Main;

namespace BabyBus.Data.Repositories.Main
{
    public class ClassRepository : RepositoryBase<Class>, IClassRepository
    {
        public ClassRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
    }
    public interface IClassRepository : IRepository<Class>
    {
    }
}
