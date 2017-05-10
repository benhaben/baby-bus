using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Helpers;
using BabyBus.Models;
using BabyBus.Models.Attendance;
using BabyBus.Models.Enums;
using BabyBus.Services;
using BabyBus.Utilities;
using Newtonsoft.Json;
using System.Threading;

namespace BabyBus.Net.Attendance {
    public interface IAttendanceService {
        Task<ApiResult<ChildModel>> GetAttendanceChildList(int classId, DateTime date, AttenceType type = AttenceType.All);

        Task<List<AttendanceMasterModel>> GetTodayAttendanceMaster(int classId);

        Task<ApiResponser> WorkAttendance(AttendanceMasterModel model);

        Task<ApiResult<DateTime>> GetChildMonthAttendance(int childId, DateTime date);
    }

    public class AttendanceService : IAttendanceService {
        private readonly RestHelper restHeler = new RestHelper();

        public async Task<ApiResult<ChildModel>> GetAttendanceChildList(int classId, DateTime date, AttenceType type) {
            string url = string.Format(Constants.GetAttendanceChildList, classId, date, (int)type);
            string json = await restHeler.GetJsonString(url);
            var result = JsonConvert.DeserializeObject<ApiResult<ChildModel>>(json);
            if (result == null) {
                return new ApiResult<ChildModel> {
                    Status = false,
                    Message = "服务器返回的数据格式错误。"
                };
            }
            return result;
        }

        public async Task<List<AttendanceMasterModel>> GetTodayAttendanceMaster(int classId) {

            #if DEBUG1
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            Task taskDelay1 = Task.Delay(5000, token);
            taskDelay1.Wait();
            #endif

            var result = await GetAttendanceMasterList(classId, DateTime.Now.Date);
            if (result.Status && result.Items != null) {
                return result.Items;
            } else {
                return new List<AttendanceMasterModel>();
            }
        }

        private async Task<ApiResult<AttendanceMasterModel>> GetAttendanceMasterList(int classId, DateTime date) {
            string url = string.Format(Constants.GetAttendanceMasterList, BabyBusContext.KindergartenId, classId, date);
            string json = await restHeler.GetJsonString(url);
            var result = JsonConvert.DeserializeObject<ApiResult<AttendanceMasterModel>>(json);
            if (result == null) {
                return new ApiResult<AttendanceMasterModel> {
                    Status = false,
                    Message = "服务器返回的数据格式错误。"
                };
            }
            return result;
        }

        public async Task<ApiResponser> WorkAttendance(AttendanceMasterModel model) {
            var response = await restHeler.AsyncUpdate(Constants.WorkAttendance, model);
            return response;
        }

        public async Task<ApiResult<DateTime>> GetChildMonthAttendance(int childId, DateTime date) {
            string url = string.Format(Constants.GetChildMonthAttendance, childId, date);
            string json = await restHeler.GetJsonString(url);
            var result = JsonConvert.DeserializeObject<ApiResult<DateTime>>(json);
            if (result == null) {
                return new ApiResult<DateTime> {
                    Status = false,
                    Message = "服务器返回的数据格式错误。"
                };
            }
            return result;
        }
    }
}
