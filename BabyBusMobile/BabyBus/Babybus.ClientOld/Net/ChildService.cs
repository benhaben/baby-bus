using System.Threading.Tasks;

using BabyBus.Logic.Shared;
using Newtonsoft.Json;


namespace BabyBus.Logic.Shared
{
    
    public class ChildService : IChildService
    {
        private readonly RestHelper restHelper = new RestHelper();

        public async Task<ApiResult<ChildModel>> GetByClassId(int classId)
        {
            string url = string.Format(Constants.GetChildrenByClassId, classId);
            string json = await restHelper.GetJsonString(url);
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
    }
}