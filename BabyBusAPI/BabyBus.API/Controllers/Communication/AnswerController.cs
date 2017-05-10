using System;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using BabyBus.API.Utils;
using BabyBus.Core.Helper;
using BabyBus.Model.Entities.FAQ;
using BabyBus.Model.Entities.Login.Enum;
using BabyBus.Service.General;
using BabyBus.Service.General.FAQ;
using BabyBus.Service.Models;

namespace BabyBus.API.Controllers.Communication {
    public class AnswerController : ApiController {
        private readonly IAnswerService _answerService;
        private readonly IParentChildRelationService _pcrService;
        private readonly IQuestionService _questionService;

        public AnswerController(IAnswerService a, IQuestionService q, IParentChildRelationService pcrService) {
            _answerService = a;
            _questionService = q;
            _pcrService = pcrService;
        }

        //post:api/Answer
        public ApiResponser Post(AnswerModel model) {
            var response = new ApiResponser(true);
            if (model.RoleType == RoleType.Teacher) {
                try {
                    model.CreateTime = DateTime.Now;
                    var answer = new Answer();
                    Mapper.DynamicMap(model, answer);
                    _answerService.CreateAnswer(answer);
                    model.AnswerId = answer.AnswerId;
                    response.Attach = model;

                    string[] user = (from q in _questionService.GetAllQuestion()
                        join pcr in _pcrService.GetAllParentChildRelation()
                            on q.ChildId equals pcr.ChildId
                        where q.QuestionId == model.QuestionId
                        select pcr.UserId.ToString()).ToArray();
                    if (user.Length > 0) {
                        JPushUtils.PushAnswer(model.Content, user, model.QuestionId);
                    }
                }
                catch (Exception ex) {
                    response.Status = false;
                    response.Message = ex.Message;
                    return response;
                }
            }
            else {
                response.Status = false;
                response.Message = "您不是教师，不能回答问题";
            }
            return response;
        }
    }
}