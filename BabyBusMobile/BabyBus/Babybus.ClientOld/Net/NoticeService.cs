using System;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace BabyBus.Logic.Shared
{
    public class NoticeService : INoticeService
    {
        private RestHelper restHeler = new RestHelper();

        public async Task<ApiResponser> SendNotice(NoticeModel model)
        {
            var response = await restHeler.AsyncUpdate(Constants.SendNotice, model);
            return response;
        }

        public async Task<ApiResult<NoticeModel>> GetNoticeById(User user, NoticeViewType viewType, long noticeID)
        {
            string url;
            var id = (user.RoleType == RoleType.Parent ? user.ChildId : user.UserId);
            url = string.Format(Constants.GetNoticeById, (int)user.RoleType, id, (int)viewType, noticeID);
            string json = await restHeler.GetJsonString(url);
            var result = JsonConvert.DeserializeObject<ApiResult<NoticeModel>>(json);
            if (result == null)
            {
                return new ApiResult<NoticeModel>
                {
                    Status = false,
                    Message = "服务器返回的数据格式错误。"
                };
            }
            return result;
        }

        public async Task<ApiResult<NoticeReadModel>> GetNoticeReadById(NoticeViewType viewType, long noticeId)
        {
            string url;
            var user = BabyBusContext.UserAllInfo;
            var id = (user.RoleType == RoleType.Parent ? user.ChildId : user.UserId);
            url = string.Format(Constants.GetNoticeReadById, (int)user.RoleType, id, (int)viewType, noticeId);
            string json = await restHeler.GetJsonString(url);
            var result = JsonConvert.DeserializeObject<ApiResult<NoticeReadModel>>(json);
            if (result == null)
            {
                return new ApiResult<NoticeReadModel>
                {
                    Status = false,
                    Message = "服务器返回的数据格式错误。"
                };
            }
            return result;
        }

        public async Task<ApiResult<NoticeModel>> GetNewNoticeList(User user, NoticeViewType viewType, long maxId = 0)
        {
            string url;
            var id = (user.RoleType == RoleType.Parent ? user.ChildId : user.UserId);
            if (maxId == 0)
            { //Get Top N Newest
                url = string.Format(Constants.TopNewNotices, (int)user.RoleType, id, (int)viewType, Constants.PAGESIZE);
            }
            else
            {
                url = string.Format(Constants.NewNotices, (int)user.RoleType, id, (int)viewType, maxId);
            }
            string json = await restHeler.GetJsonString(url);
            var result = JsonConvert.DeserializeObject<ApiResult<NoticeModel>>(json);
            if (result == null)
            {
                return new ApiResult<NoticeModel>
                {
                    Status = false,
                    Message = "服务器返回的数据格式错误。"
                };
            }
            return result;
        }

        public async Task<ApiResult<NoticeModel>> GetOldNoticeList(User user, NoticeViewType viewType, long minId)
        {
            
            var id = (user.RoleType == RoleType.Parent ? user.ChildId : user.UserId);
            string url = string.Format(Constants.TopOldNotices, (int)user.RoleType, id, (int)viewType
                , Constants.PAGESIZE, minId);
            string json = await restHeler.GetJsonString(url);
            var result = JsonConvert.DeserializeObject<ApiResult<NoticeModel>>(json);
            if (result == null)
            {
                return new ApiResult<NoticeModel>
                {
                    Status = false,
                    Message = "服务器返回的数据格式错误。"
                };
            }
            return result;
        }

        public async Task<ApiResult<ChildModel>> GetReadListByNoticeId(long noticeId)
        {
            string url = string.Format(Constants.GetReadListByNoticeId, noticeId);
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
    }
}