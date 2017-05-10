using BabyBus.Data.Infrastructure;
using BabyBus.Model.Entities.Login;

namespace BabyBus.Data.Repositories.Login
{
    public class AdminRepository : RepositoryBase<Admin>, IAdminRepository
    {
        public AdminRepository(IDatabaseFactory databaseFactory) : base (databaseFactory)
        {
        }
    }

    public interface IAdminRepository : IRepository<Admin>
    {
    }
}