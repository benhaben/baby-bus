using BabyBus.Data.Infrastructure;
using BabyBus.Model.Entities.Other;

namespace BabyBus.Data.Repositories.Other
{
    public class WeChatInfoRepository : RepositoryBase<WeChatInfo>, IWeChatInfoRepository
    {
        public WeChatInfoRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
    }
    public interface IWeChatInfoRepository : IRepository<WeChatInfo>
    {
    }
}