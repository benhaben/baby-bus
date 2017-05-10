using BabyBus.Data.Infrastructure;
using BabyBus.Model.Entities.Article;

namespace BabyBus.Data.Repositories.Article
{
    public class NoticeRepository : RepositoryBase<Notice>, INoticeRepository
    {
        public NoticeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
    }
    public interface INoticeRepository : IRepository<Notice>
    {
    }
}
