using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Helpers;
using BabyBus.Models;
using BabyBus.Models.GroupMemory;
using BabyBus.Models.Main;
using Newtonsoft.Json;

namespace BabyBus.Services.Main
{
    public class MainService : IMainService
    {
        private RestHelper restHelper = new RestHelper();
//        public async Task<ChildInfo> GetChildById(int childId)
//        {
//            var url = string.Format(Constants.ChildDetail, childId);
//
//            var result = await restHelper.AsyncQuery<ChildInfo>(url);
//            if (result.Status && result.Items != null && result.Items.Count > 0)
//            {
//                var child = result.Items[0];
//                child.Age = 3;
//                child.Height = 98;
//                child.Weight = 23;
//				child.Score = 3.2;
//                return child;
//            }
//            else
//            {
//                return new ChildInfo();
//            }
//        }

//        public async Task<ChildAllInfo> GetChildAllInfoByParentId(int userId)
//        {
//            var url = string.Format(Constants.ChildAllInfo, userId);
//
//            var result = await restHelper.AsyncQuery<ChildAllInfo>(url);
//            if (result.Status && result.Items != null && result.Items.Count > 0)
//            {
//                var child = result.Items[0];
//                return child;
//            }
//            return new ChildAllInfo();
//        }

//        public async Task<List<Article>> GetNoticeList(int userId)
//        {
//            var url = string.Format(Constants.ClassArtList, userId);
//
//            var result = await restHelper.AsyncQuery<Article>(url);
//            if (result.Status && result.Items != null)
//            {
//                return result.Items;
//            }
//            return new List<Article>();
//        }

        public async Task<bool> Logout(int userId)
        {
            //Logout User
			await Task.Delay (1);
            return true;
        }

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

        public async Task<ApiResult<VersionModel>> GetVersions(int appType) {
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
                return new ApiResult<VersionModel> {
                    Status = false,
                    Message = "服务器返回的数据格式错误。"
                };
            }
            return result;
        }

        public async Task<VersionModel> GetVersionByName(int appType) {
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

        public async Task<ChildSummaryInfo> GetChildSummary(int childId) {
            string url = string.Format(Constants.ChildSummary, childId);

            ApiResult<ChildSummaryInfo> result=new ApiResult<ChildSummaryInfo>();

            try {
                string json = await restHelper.GetJsonString(url);
                result = JsonConvert.DeserializeObject<ApiResult<ChildSummaryInfo>>(json);
            }
            catch (Exception ex) {
            }
            if (result.Items != null && result.Items.Count > 0) {
                return result.Items[0];
            }
            return new ChildSummaryInfo();
        }
    }
}
