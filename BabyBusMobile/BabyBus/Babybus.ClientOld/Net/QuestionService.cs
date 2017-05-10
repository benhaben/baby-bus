using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using BabyBus.Logic.Shared;
using Newtonsoft.Json;



namespace BabyBus.Logic.Shared
{
    public class QuestionService : IQuestionService
    {
        private RestHelper restHeler = new RestHelper();

        public async Task<ApiResult<QuestionModel>> GetQuestionById(User user, long questionId)
        {
            string url;
            var id = user.UserId;
            url = string.Format(Constants.GetQuestionById, (int)user.RoleType, id, questionId);
            string json = await restHeler.GetJsonString(url);
            var result = JsonConvert.DeserializeObject<ApiResult<QuestionModel>>(json);
            if (result == null)
            {
                return new ApiResult<QuestionModel>
                {
                    Status = false,
                    Message = "服务器返回的数据格式错误。"
                };
            }
            return result;
        }


        public async Task<ApiResponser> SendQuestion(QuestionModel model)
        {
            var response = await restHeler.AsyncUpdate(Constants.SendQuestion, model);
            return response;
        }

        public async Task<ApiResponser> SendAnswer(AnswerModel model)
        {
            var response = await restHeler.AsyncUpdate(Constants.SendAnswer, model);
            return response;
        }

        public async Task<ApiResult<QuestionModel>> GetNewQuestionList(User user, long maxId = 0)
        {
            string url;
            if (maxId == 0)
            {
                url = string.Format(Constants.TopNewQuestions, (int)user.RoleType, user.UserId, Constants.PAGESIZE);
            }
            else
            {
                url = string.Format(Constants.NewQuestions, (int)user.RoleType, user.UserId, maxId);
            }
            string json = await restHeler.GetJsonString(url);
            var result = JsonConvert.DeserializeObject<ApiResult<QuestionModel>>(json);
            if (result == null)
            {
                return new ApiResult<QuestionModel>
                {
                    Status = false,
                    Message = "服务器返回的数据格式错误。"
                };
            }
            return result;
        }

        public async Task<ApiResult<QuestionModel>> GetOldQuestionList(User user, long minId)
        {
            string url = string.Format(Constants.TopOldQuestions, (int)user.RoleType, user.UserId, Constants.PAGESIZE, minId);
            string json = await restHeler.GetJsonString(url);
            var result = JsonConvert.DeserializeObject<ApiResult<QuestionModel>>(json);
            if (result == null)
            {
                return new ApiResult<QuestionModel>
                {
                    Status = false,
                    Message = "服务器返回的数据格式错误。"
                };
            }
            return result;
        }
    }
}
