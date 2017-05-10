using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Data.Infrastructure;
using BabyBus.Model.Entities.Summary;

namespace BabyBus.Data.Repositories.Summary
{
    public class ClassMonthSummaryRepository:RepositoryBase<ClassMonthSummary>,IClassMonthSummaryRepository {
        public ClassMonthSummaryRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) {
        }
    }

    public interface IClassMonthSummaryRepository : IRepository<ClassMonthSummary> {
        
    }
}
