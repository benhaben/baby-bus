using BabyBus.Data.Infrastructure;
using BabyBus.Model.Entities.Login;

namespace BabyBus.Data.Repositories.Login
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
    }
    public interface IUserRepository : IRepository<User>
    {
    }
}