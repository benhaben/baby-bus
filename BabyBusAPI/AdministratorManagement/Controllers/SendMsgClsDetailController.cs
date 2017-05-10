using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BabyBus.AutoModel;
using BabyBus.Core.Helper;
using System.Web.Http;
namespace AdministratorManagement.Controllers
{
    public class SendMsgClsDetailController : BabyBusApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(ManagementController));

       
        public class MessageRequest
        {
            public int Year { get; set; }
            public int Month { get; set; }
            public int KingdergartendId { get; set; }
            public int ClassId { get; set; }
            public int NoticeType { get; set; }
            public int QusetionType { get; set; }
            public string typeVal { get; set; }
        }

        
        private ApiResponser msgRecdsClsesInfo(MessageRequest value)
        {
            StartWatch();
            var response = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {

                    if (value.typeVal == "all")
                    {
                        //查询notice
                        var msgInfo = from n in db.Notice
                                      where n.KindergartenId == value.KingdergartendId
                                      && n.CreateTime.Year == value.Year
                                      && n.CreateTime.Month == value.Month
                                      && n.ClassId == value.ClassId
                                      orderby n.NoticeType
                                      group n by n.NoticeType into g
                                      select new
                                      {
                                          typeNo = g.Key,
                                          typeInfo = from ni in db.Notice
                                                     where ni.KindergartenId == value.KingdergartendId
                                                      && ni.CreateTime.Year == value.Year
                                                      && ni.CreateTime.Month == value.Month
                                                      && ni.ClassId == value.ClassId
                                                      && ni.NoticeType == g.Key
                                                     orderby ni.CreateTime descending
                                                     select new
                                                     {
                                                         msg = ni,
                                                         year = ni.CreateTime.Year,
                                                         month = ni.CreateTime.Month,
                                                         day = ni.CreateTime.Day,
                                                         userName = from u in db.Users
                                                                    where u.UserId == ni.UserId
                                                                    select u.RealName
                                                     },
                                      };
                        //查询question
                        if (value.ClassId == 0)
                        {
                            //只查询园长提问,type=2(固定值)
                            var questions = from q in db.Question
                                            join cd in db.Child
                                            on q.ChildId equals cd.ChildId
                                            where cd.KindergartenId == value.KingdergartendId
                                            && q.CreateTime.Year == value.Year
                                            && q.CreateTime.Month == value.Month
                                            && q.QuestionType == 2
                                            group q by q.QuestionType into g
                                            select new 
                                            { 
                                                typeNo = g.Key,
                                                typeInfo = from q in db.Question
                                                           join cd in db.Child
                                                           on q.ChildId equals cd.ChildId
                                                           where cd.KindergartenId == value.KingdergartendId
                                                           && q.CreateTime.Year == value.Year
                                                           && q.CreateTime.Month == value.Month
                                                           && q.QuestionType == g.Key
                                                           select new
                                                           {
                                                               question = q,
                                                               year = q.CreateTime.Year,
                                                               month = q.CreateTime.Month,
                                                               day = q.CreateTime.Day,
                                                           }
                                            };
                            response.Attach = new
                            {
                                questionInfo = questions.ToList(),
                                messageInfo = msgInfo.ToList(),
                            };
                            return response;
 
                        }
                        else 
                        { 
                            //查询教师提问和请假（教师提问：1，请假：0）
                            var questions = from q in db.Question
                                            join cd in db.Child
                                            on q.ChildId equals cd.ChildId
                                            where cd.ClassId == value.ClassId
                                            && q.CreateTime.Year == value.Year
                                            && q.CreateTime.Month == value.Month
                                            && q.QuestionType != 2
                                            group q by q.QuestionType into g
                                            select new
                                            {
                                                typeNo = g.Key,
                                                typeInfo = from q in db.Question
                                                           join cd in db.Child
                                                           on q.ChildId equals cd.ChildId
                                                           where cd.ClassId == value.ClassId
                                                           && q.CreateTime.Year == value.Year
                                                           && q.CreateTime.Month == value.Month
                                                           && q.QuestionType == g.Key
                                                           select new
                                                           {
                                                               question = q,
                                                               year = q.CreateTime.Year,
                                                               month = q.CreateTime.Month,
                                                               day = q.CreateTime.Day,
                                                           }

                                            };
                            response.Attach = new
                            {
                                questionInfo = questions.ToList(),
                                messageInfo = msgInfo.ToList(),
                            };
                            return response;

                        }
                    }
                    else if (value.typeVal == "question")
                    {
                        if (value.QusetionType == 2) { 
                            var questions = from q in db.Question
                                            join cd in db.Child
                                            on q.ChildId equals cd.ChildId
                                            where cd.KindergartenId == value.KingdergartendId
                                            && q.CreateTime.Year == value.Year
                                            && q.CreateTime.Month == value.Month
                                            && q.QuestionType == value.QusetionType
                                            group q by q.QuestionType into g
                                            select new
                                            {
                                                typeNo = g.Key,
                                                typeInfo = from q in db.Question
                                                           join cd in db.Child
                                                           on q.ChildId equals cd.ChildId
                                                           where cd.KindergartenId == value.KingdergartendId
                                                           && q.CreateTime.Year == value.Year
                                                           && q.CreateTime.Month == value.Month
                                                           && q.QuestionType == g.Key
                                                           select new
                                                           {
                                                               question = q,
                                                               year = q.CreateTime.Year,
                                                               month = q.CreateTime.Month,
                                                               day = q.CreateTime.Day,
                                                           }
                                            };
                            response.Attach = new
                            {
                                questionInfo = questions.ToList(),
                            };
                        }
                        else
                        {
                            var questions = from q in db.Question
                                            join cd in db.Child
                                            on q.ChildId equals cd.ChildId
                                            where cd.ClassId == value.ClassId
                                            && q.CreateTime.Year == value.Year
                                            && q.CreateTime.Month == value.Month
                                            && q.QuestionType == value.QusetionType
                                            group q by q.QuestionType into g
                                            select new
                                            {
                                                typeNo = g.Key,
                                                typeInfo = from q in db.Question
                                                           join cd in db.Child
                                                           on q.ChildId equals cd.ChildId
                                                           where cd.ClassId == value.ClassId
                                                           && q.CreateTime.Year == value.Year
                                                           && q.CreateTime.Month == value.Month
                                                           && q.QuestionType == g.Key
                                                           select new
                                                           {
                                                               question = q,
                                                               year = q.CreateTime.Year,
                                                               month = q.CreateTime.Month,
                                                               day = q.CreateTime.Day,
                                                           }
                                            };
                            response.Attach = new
                            {
                                questionInfo = questions.ToList(),
                            };
                        }
                        return response;
                    }
                    else if (value.typeVal == "message")
                    {
                        //查询notice
                        var msgInfo = from n in db.Notice
                                      where n.KindergartenId == value.KingdergartendId
                                      && n.CreateTime.Year == value.Year
                                      && n.CreateTime.Month == value.Month
                                      && n.ClassId == value.ClassId
                                      && n.NoticeType == value.NoticeType
                                      orderby n.NoticeType
                                      group n by n.NoticeType into g
                                      select new
                                      {
                                          typeNo = g.Key,
                                          typeInfo = from ni in db.Notice
                                                     where ni.KindergartenId == value.KingdergartendId
                                                      && ni.CreateTime.Year == value.Year
                                                      && ni.CreateTime.Month == value.Month
                                                      && ni.ClassId == value.ClassId
                                                      && ni.NoticeType == g.Key
                                                      && ni.NoticeType == value.NoticeType
                                                     orderby ni.CreateTime descending
                                                     select new
                                                     {
                                                         msg = ni,
                                                         year = ni.CreateTime.Year,
                                                         month = ni.CreateTime.Month,
                                                         day = ni.CreateTime.Day,
                                                         userName = from u in db.Users
                                                                    where u.UserId == ni.UserId
                                                                    select u.RealName
                                                     },
                                      };
                        response.Attach = new
                        {
                            messageInfo = msgInfo.ToList(),
                        };
                        return response;
                    }

                }//using end
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
                Log.Fatal(ex.Message, ex);
                return response;
            }

            EndWatch();
            return response;
        }


        [System.Web.Http.HttpPost]
        public ApiResponser Post(MessageRequest request)
        {
            return msgRecdsClsesInfo(request);
        }
	}
}