using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Data.Infrastructure;
using BabyBus.Model.Entities.Main;
using BabyBus.Model.Entities.Summary;

namespace BabyBus.Data.Repositories.Summary
{
    public class ClassDaySummaryRepository : RepositoryBase<ClassDaySummary>,IClassDaySummaryRepository {
        public ClassDaySummaryRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) {
        }
    }

    public interface IClassDaySummaryRepository : IRepository<ClassDaySummary> {
        
    }
}
