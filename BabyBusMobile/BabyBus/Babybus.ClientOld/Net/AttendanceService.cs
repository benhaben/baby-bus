using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BabyBus.Logic.Shared
{
    public class AttendanceService : IAttendanceService
    {
        private readonly RestHelper restHeler = new RestHelper();

        public async Task<ApiResult<ChildModel>> GetAttendanceChildList(int classId, DateTime date, AttenceType type)
        {
            string url = string.Format(Constants.GetAttendanceChildList, classId, date.Date, (int)type);
            string json = await restHeler.GetJsonString(url);
            var result = JsonConvert.DeserializeObject<ApiResult<ChildModel>>(json);
            if (result == null)
            {
                return new ApiResult<ChildModel>
                {
                    Status = false,
                    Message = "服务器返回的数据格式错误。"
                };
            }
            return result;
        }

        public async Task<List<AttendanceMasterModel>> GetAttendanceMasterList(int classId, DateTime date)
        {
            #if DEBUG1
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            Task taskDelay1 = Task.Delay(5000, token);
            taskDelay1.Wait();
            #endif
            string url = string.Format(Constants.GetAttendanceMasterList, BabyBusContext.KindergartenId, classId, date.Date);
            string json = await restHeler.GetJsonString(url);
            var result = JsonConvert.DeserializeObject<ApiResult<AttendanceMasterModel>>(json);
           
            if (result != null && result.Status && result.Items != null)
            {
                return result.Items;
            }
            else
            {
                return new List<AttendanceMasterModel>();
            }
        }

        public async Task<ApiResponser> WorkAttendance(AttendanceMasterModel model)
        {
            var response = await restHeler.AsyncUpdate(Constants.WorkAttendance, model);
            return response;
        }

        public async Task<ApiResult<DateTime>> GetChildMonthAttendance(int childId, DateTime date)
        {
            string url = string.Format(Constants.GetChildMonthAttendance, childId, date.Date);
            string json = await restHeler.GetJsonString(url);
            var result = JsonConvert.DeserializeObject<ApiResult<DateTime>>(json);
            if (result == null)
            {
                return new ApiResult<DateTime>
                {
                    Status = false,
                    Message = "服务器返回的数据格式错误。"
                };
            }
            return result;
        }
    }
}
