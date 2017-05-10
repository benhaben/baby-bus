using System;
using System.Threading.Tasks;



using Newtonsoft.Json;
using Cirrious.CrossCore;

namespace BabyBus.Logic.Shared
{
    public class MainService : IMainService
    {
        private RestHelper restHelper = new RestHelper();

       

        public async Task<ClassInfo> GetClassByTeacherId(int userId)
        {
            await Task.Delay(1);
            var classInfo = new ClassInfo
            {
                Total = 24,
                Online = 20,
                Leave = 1,
                UnRegister = 3
            };
            return classInfo;
        }

        public async Task<ApiResponser> ChangePassword(object pwd)
        {
            var url = Constants.ChangePassword;
            var result = await restHelper.AsyncUpdate(url, pwd);
            return result;
        }

        public async Task<ApiResponser> SendFeedBack(BabyBus.Logic.Shared.FeedBack feedBack)
        {
            var url = Constants.FeedBack;
            var result = await restHelper.AsyncUpdate(url, feedBack);
            return result;
        }

        public async Task<ApiResult<VersionModel>> GetVersions(int appType)
        {
            string url = string.Format(Constants.Versions, appType);
            ApiResult<VersionModel> result;
            try
            {
                string json = await restHelper.GetJsonString(url);
                result = JsonConvert.DeserializeObject<ApiResult<VersionModel>>(json);
                if (result == null)
                {
                    return new ApiResult<VersionModel>
                    {
                        Status = false,
                        Message = "服务器返回的数据格式错误。"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResult<VersionModel>
                {
                    Status = false,
                    Message = "服务器返回的数据格式错误。"
                };
            }
            return result;
        }

        public async Task<VersionModel> GetVersionByName(int appType)
        {
            var result = await GetVersions(appType);
            if (result != null && result.Items != null && result.Items.Count > 0)
            {
                var version = result.Items[0];
                return version;
            }
            else
            {
                return null;
            }
        }

        public async Task<ChildSummaryInfo> GetChildSummary(int childId)
        {
            string url = string.Format(Constants.ChildSummary, childId);

            ApiResult<ChildSummaryInfo> result = new ApiResult<ChildSummaryInfo>();

            try
            {
                string json = await restHelper.GetJsonString(url);
                result = JsonConvert.DeserializeObject<ApiResult<ChildSummaryInfo>>(json);
            }
            catch (Exception ex)
            {
                Mvx.Trace(ex.Message);
            }
            if (result.Items != null && result.Items.Count > 0)
            {
                return result.Items[0];
            }
            return new ChildSummaryInfo();
        }
    }
}
