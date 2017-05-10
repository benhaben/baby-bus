using BabyBus.Data.Infrastructure;
using BabyBus.Model.Entities.Attendance;

namespace BabyBus.Data.Repositories.Attendance {
    public class AttendanceMasterRepository : RepositoryBase<AttendanceMaster>, IAttendanceMasterRepository {
        public AttendanceMasterRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory) {
        }
    }

    public interface IAttendanceMasterRepository : IRepository<AttendanceMaster> {
    }
}