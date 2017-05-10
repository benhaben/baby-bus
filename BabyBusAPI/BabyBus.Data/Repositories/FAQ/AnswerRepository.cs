using BabyBus.Data.Infrastructure;
using BabyBus.Model.Entities.FAQ;

namespace BabyBus.Data.Repositories.FAQ
{
    public class AnswerRepository : RepositoryBase<Answer>, IAnswerRepository
    {
        public AnswerRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    public interface IAnswerRepository : IRepository<Answer>
    {
    }
}
