using System.Threading.Tasks;
using BabyBus.Logic.Shared;
using Newtonsoft.Json;

namespace BabyBus.Logic.Shared
{
    public class CommentService : ICommentService
    {
        private RestHelper restHeler = new RestHelper();

        public async Task<ApiResponser> SendComment(CommentModel model)
        {
            var response = await restHeler.AsyncUpdate(Constants.SendComment, model);
            return response;
        }

        public async Task<ApiResult<CommentModel>>GetCommentListByNoticeId(long  noticeId)
        {
            string url;
            url = string.Format(Constants.GetCommentByNoticeId, noticeId);
            string json = await restHeler.GetJsonString(url);

            var result = JsonConvert.DeserializeObject<ApiResult<CommentModel>>(json);
            if (result == null)
            {
                return new ApiResult<CommentModel>
                {
                    Status = false,
                    Message = "服务器返回的数据格式错误。"
                };
            }
            return result;
        }
    }
}

