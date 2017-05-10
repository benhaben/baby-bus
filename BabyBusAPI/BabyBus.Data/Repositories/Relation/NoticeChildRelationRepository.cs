using BabyBus.Data.Infrastructure;
using BabyBus.Model.Entities.Relation;

namespace BabyBus.Data.Repositories.Relation
{
    public class NoticeChildRelationRepository : RepositoryBase<NoticeChildRelation>, INoticeChildRelationRepository
    {
        public NoticeChildRelationRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
    }
    public interface INoticeChildRelationRepository : IRepository<NoticeChildRelation>
    {
    }
}
