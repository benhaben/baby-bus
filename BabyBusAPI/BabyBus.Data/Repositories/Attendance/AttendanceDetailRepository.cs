using BabyBus.Data.Infrastructure;
using BabyBus.Model.Entities.Attendance;

namespace BabyBus.Data.Repositories.Attendance {
    public class AttendanceDetailRepository : RepositoryBase<AttendanceDetail>, IAttendanceDetailRepository {
        public AttendanceDetailRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory) {
        }
    }

    public interface IAttendanceDetailRepository : IRepository<AttendanceDetail> {
    }
}