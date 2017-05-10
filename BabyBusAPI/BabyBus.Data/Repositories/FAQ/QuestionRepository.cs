using BabyBus.Data.Infrastructure;
using BabyBus.Model.Entities.FAQ;

namespace BabyBus.Data.Repositories.FAQ
{
    public class QuestionRepository : RepositoryBase<Question>, IQuestionRepository
    {
        public QuestionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
    }
    public interface IQuestionRepository : IRepository<Question>
    {
    }
}
