using BabyBus.Data.Infrastructure;
using BabyBus.Model.Entities.Login;

namespace BabyBus.Data.Repositories.Login
{
    public class CardPasswordRepository : RepositoryBase<CardPassword>, ICardPasswordRepository
    {
        public CardPasswordRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
    }
    public interface ICardPasswordRepository : IRepository<CardPassword>
    {
    }
}
