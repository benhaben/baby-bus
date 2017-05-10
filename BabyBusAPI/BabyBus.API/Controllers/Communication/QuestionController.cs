using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData.Query;
using AutoMapper;
using BabyBus.API.Utils;
using BabyBus.Core.Helper;
using BabyBus.Model.Entities.FAQ;
using BabyBus.Model.Entities.Login.Enum;
using BabyBus.Model.Entities.Relation;
using BabyBus.Service.General;
using BabyBus.Service.General.FAQ;
using BabyBus.Service.General.Main;
using BabyBus.Service.Models;

namespace BabyBus.API.Controllers.Communication {
    public class QuestionController : ApiController {
        private readonly IAnswerService _answerService;
        private readonly IChildService _childService;
        private readonly IParentChildRelationService _pcrService;
        private readonly IQuestionService _questionService;
        private readonly IUserService _userService;


        public QuestionController(IQuestionService q, IParentChildRelationService p, IChildService c,
            IUserService u, IAnswerService a) {
            _questionService = q;
            _pcrService = p;
            _childService = c;
            _userService = u;
            _answerService = a;
        }

        //Post:api/Question
        [HttpPost]
        public ApiResponser Post(QuestionModel model) {
            var response = new ApiResponser(true);
            if (model.RoleType == RoleType.Parent) {
                try {
                    model.CreateTime = DateTime.Now;
                    var question = new Question();
                    Mapper.DynamicMap(model, question);
                    _questionService.CreateQuestion(question);
                    response.Attach = model;

                    //Push
                    string[] users = (from c in _childService.GetAllChild()
                        join u in _userService.GetAllUser() on c.ClassId equals u.ClassId
                        where c.ChildId == model.ChildId && u.RoleType == RoleType.Teacher
                        select u.UserId.ToString()).ToArray();
                    if (users.Length > 0) {
                        JPushUtils.PushQuestion(model.Content, users, question.QuestionId);
                    }
                }
                catch (Exception ex) {
                    response.Status = false;
                    response.Message = ex.Message;
                    return response;
                }
            }
            return response;
        }

        //Get:api/Question
        [HttpGet]
        public ApiResult<Question> Get(RoleType type, int id, ODataQueryOptions<Question> options) {
            var result = new ApiResult<Question>();
            List<QuestionModel> enumerable = null;

            if (type == RoleType.Parent) {
                ParentChildRelation parentChildRelation =
                    _pcrService.GetAllParentChildRelation().FirstOrDefault(x => x.UserId == id);
                int childId = 0;
                if (parentChildRelation != null) {
                    childId =
                        parentChildRelation
                            .ChildId;
                }
                IQueryable<Question> questionList = _questionService.GetAllQuestion().Where(x => x.ChildId == childId);
                questionList = options.ApplyTo(questionList) as IQueryable<Question>;

                IQueryable<QuestionModel> list = from q in questionList
                    join c in _childService.GetAllChild() on q.ChildId equals c.ChildId
                    orderby q.QuestionId descending
                    select new QuestionModel {
                        QuestionId = q.QuestionId,
                        ChildId = q.ChildId,
                        ChildName = c.ChildName,
                        Content = q.Content,
                        CreateTime = q.CreateTime,
                        QuestionType = q.QuestionType,
                    };
                enumerable = list.ToList();
                List<AnswerModel> answers = (from a in _answerService.GetAllAnswer()
                    join q in questionList on a.QuestionId equals q.QuestionId
                    join u in _userService.GetAllUser() on a.UserId equals u.UserId
                    orderby a.AnswerId descending
                    select new AnswerModel {
                        AnswerId = a.AnswerId,
                        Content = a.Content,
                        CreateTime = a.CreateTime,
                        QuestionId = a.QuestionId,
                        UserId = a.UserId,
                        UserName = u.RealName
                    }).ToList();

                foreach (QuestionModel q in enumerable) {
                    IEnumerable<AnswerModel> temp = answers.Where(x => x.QuestionId == q.QuestionId);
                    if (temp.Any()) {
                        q.TeacherName = temp.FirstOrDefault().UserName;
                        q.AnswerContent = temp.FirstOrDefault().Content;
                        q.Answers = temp;
                    }
                }
            }
            else if (type == RoleType.Teacher) {
                var questionList = from q in _questionService.GetAllQuestion()
                    join c in _childService.GetAllChild() on q.ChildId equals c.ChildId
                    join u in _userService.GetAllUser() on c.ClassId equals u.ClassId
                    where u.UserId == id
                    select q;

                questionList = options.ApplyTo(questionList) as IQueryable<Question>;

                var list = from q in questionList
                    join c in _childService.GetAllChild() on q.ChildId equals c.ChildId
                    orderby q.QuestionId descending
                    select new QuestionModel {
                        QuestionId = q.QuestionId,
                        ChildId = q.ChildId,
                        ChildName = c.ChildName,
                        Content = q.Content,
                        CreateTime = q.CreateTime,
                        QuestionType = q.QuestionType
                    };

                enumerable = list.ToList();

                var answers = (from a in _answerService.GetAllAnswer()
                    join q in questionList on a.QuestionId equals q.QuestionId
                    join u in _userService.GetAllUser() on a.UserId equals u.UserId
                    orderby a.AnswerId descending
                    select new AnswerModel {
                        AnswerId = a.AnswerId,
                        Content = a.Content,
                        CreateTime = a.CreateTime,
                        QuestionId = a.QuestionId,
                        UserId = a.UserId,
                        UserName = u.RealName
                    }).ToList();

                foreach (var q in enumerable) {
                    var temp = answers.Where(x => x.QuestionId == q.QuestionId);
                    if (temp.Any()) {
                        q.TeacherName = temp.FirstOrDefault().UserName;
                        q.AnswerContent = temp.FirstOrDefault().Content;
                        q.Answers = temp;
                    }
                }
            }
            else {
                result.Status = false;
                return result;
            }

            result.Status = true;
            result.Items = enumerable;
            result.TotalCount = enumerable.Count;
            return result;
        }
    }
}