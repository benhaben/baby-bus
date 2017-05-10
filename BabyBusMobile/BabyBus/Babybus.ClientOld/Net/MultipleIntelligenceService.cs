using System.Threading.Tasks;
using Newtonsoft.Json;


namespace BabyBus.Logic.Shared
{
    public class MultipleIntelligenceService : IMultipleIntelligenceService
    {
        RestHelper restHelper = new RestHelper();

        public async Task<ApiResult<MITestMaster>> GetParentModality(int userId)
        {
            string url;
            url = string.Format(Constants.GetParentModality, userId);
            string json = await restHelper.GetJsonString(url);
            var result = JsonConvert.DeserializeObject<ApiResult<MITestMaster>>(json);
            if (result == null)
            {
                return new ApiResult<MITestMaster>
                {
                    Status = false,
                    Message = "服务器返回的数据格式错误。"
                };
            }
            return result;
        }

        public async Task<ApiResult<MIAssessIndex>> GetTestDetail(RoleType roleType, int modalityId, int childId)
        {
            string url;
            url = string.Format(Constants.GetMITestDetail, (int)roleType, modalityId, childId);
            string json = await restHelper.GetJsonString(url);
            var result = JsonConvert.DeserializeObject<ApiResult<MIAssessIndex>>(json);
            if (result == null)
            {
                return new ApiResult<MIAssessIndex>
                {
                    Status = false,
                    Message = "服务器返回的数据格式错误。"
                };
            }
            return result;
        }

        public async Task<ApiResponser> SendTestQuestions(MIPostQuestions model)
        {
            var response = await restHelper.AsyncUpdate(Constants.PostMITestQuestions, model);
            return response;
        }

        public async Task<ApiResult<MIModality>> GetTeacherModality(int userId)
        {
            string url;
            url = string.Format(Constants.GetTeacherModality, userId);
            string json = await restHelper.GetJsonString(url);
            var result = JsonConvert.DeserializeObject<ApiResult<MIModality>>(json);
            if (result == null)
            {
                return new ApiResult<MIModality>
                {
                    Status = false,
                    Message = "服务器返回的数据格式错误。"
                };
            }
            return result;
        }

        public async Task<ApiResult<MITestMaster>> GetModalityChild(int modalityId, int userId)
        {
            string url;
            url = string.Format(Constants.GetModalityChild, modalityId, userId);
            string json = await restHelper.GetJsonString(url);
            var result = JsonConvert.DeserializeObject<ApiResult<MITestMaster>>(json);
            if (result == null)
            {
                return new ApiResult<MITestMaster>
                {
                    Status = false,
                    Message = "服务器返回的数据格式错误。"
                };
            }
            return result;
        }
    }
}

