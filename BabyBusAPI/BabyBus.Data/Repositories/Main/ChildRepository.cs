using System.Collections.Generic;
using BabyBus.Data.Infrastructure;
using BabyBus.Model.Entities.Main;

namespace BabyBus.Data.Repositories.Main
{
    public class ChildRepository : RepositoryBase<Child>, IChildRepository
    {
        public ChildRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
    }
    public interface IChildRepository : IRepository<Child>
    {
    }
}
