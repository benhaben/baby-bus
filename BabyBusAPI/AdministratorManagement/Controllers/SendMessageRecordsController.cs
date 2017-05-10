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
    public class SendMessageRecordsController : BabyBusApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(ManagementController));
        public SendMessageRecordsController()
        { 
        }

        public class MessageRequest
        {
            public int Year { get; set; }
            public int KindergartenId { get; set; }
            public int Type { get; set; }
            public int ClassId { get; set; }
        }

        private ApiResponser messageInfo(MessageRequest value)
        {
            StartWatch();
            var response = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    if (value.Type == 0)
                    {
                        var messageMonthInfo = from n in db.Notice
                                               where n.KindergartenId == value.KindergartenId
                                               && n.CreateTime.Year == value.Year
                                               group n by n.CreateTime.Month into g
                                               select new
                                               {
                                                   month = g.Key,
                                                   sumCount = g.Count(),
                                                   typeinfo = from nn in db.Notice
                                                              where nn.KindergartenId == value.KindergartenId
                                                              && nn.CreateTime.Year == value.Year
                                                              && nn.CreateTime.Month == g.Key
                                                              group nn by nn.NoticeType into gn
                                                              select new
                                                              {
                                                                  type = gn.Key,
                                                                  typeCount = gn.Count()
                                                              }
                                               };

                        var questionMonthInfo = from q in db.Question
                                                join cd in db.Child
                                                on q.ChildId equals cd.ChildId
                                                join k in db.Kindergarten
                                                on cd.KindergartenId equals k.KindergartenId
                                                where k.KindergartenId == value.KindergartenId
                                                && q.CreateTime.Year == value.Year
                                                group q by q.CreateTime.Month into g
                                                select new
                                                {
                                                    month = g.Key,
                                                    sumCount = g.Count(),
                                                    typeInfo = from q in db.Question
                                                               join cd in db.Child
                                                               on q.ChildId equals cd.ChildId
                                                               join k in db.Kindergarten
                                                               on cd.KindergartenId equals k.KindergartenId
                                                               where k.KindergartenId == value.KindergartenId
                                                               && q.CreateTime.Year == value.Year
                                                               && q.CreateTime.Month == g.Key
                                                               group q by q.QuestionType into gn
                                                               select new
                                                               {
                                                                   type = gn.Key,
                                                                   typeCount = gn.Count()
                                                               }

                                                };
                        response.Attach = new
                        {
                            messageInfo = messageMonthInfo.ToList(),
                            questionInfo = questionMonthInfo.ToList(),
                        };
                    }
                    else {
                        //排除园区消息
                        List<int> clsNoticeTypeList = new List<int> { 1, 2, 6 };
                        var messageMonthInfo = from n in db.Notice
                                               where n.KindergartenId == value.KindergartenId
                                               && n.CreateTime.Year == value.Year
                                               && n.ClassId == value.ClassId
                                               && clsNoticeTypeList.Contains(n.NoticeType)
                                               group n by n.CreateTime.Month into g
                                               select new
                                               {
                                                   month = g.Key,
                                                   sumCount = g.Count(),
                                                   typeinfo = from nn in db.Notice
                                                              where nn.KindergartenId == value.KindergartenId
                                                              && nn.CreateTime.Year == value.Year
                                                              && nn.CreateTime.Month == g.Key
                                                              && nn.ClassId == value.ClassId
                                                              && clsNoticeTypeList.Contains(nn.NoticeType)
                                                              group nn by nn.NoticeType into gn
                                                              select new
                                                              {
                                                                  type = gn.Key,
                                                                  typeCount = gn.Count()
                                                              }
                                               };
                        //0(请假),1(教师提问),2(园长提问)
                        List<int> clsQuestionInfoList = new List<int> { 0, 1 };
                        var questionMonthInfo = from q in db.Question
                                                join cd in db.Child
                                                on q.ChildId equals cd.ChildId
                                                join k in db.Kindergarten
                                                on cd.KindergartenId equals k.KindergartenId
                                                join cls in db.Classes
                                                on cd.ClassId equals cls.ClassId
                                                where k.KindergartenId == value.KindergartenId
                                                && q.CreateTime.Year == value.Year
                                                && cls.ClassId == value.ClassId
                                                && clsQuestionInfoList.Contains(q.QuestionType)
                                                group q by q.CreateTime.Month into g
                                                select new
                                                {
                                                    month = g.Key,
                                                    sumCount = g.Count(),
                                                    typeInfo = from q in db.Question
                                                               join cd in db.Child
                                                               on q.ChildId equals cd.ChildId
                                                               join k in db.Kindergarten
                                                               on cd.KindergartenId equals k.KindergartenId
                                                               join cls in db.Classes
                                                               on cd.ClassId equals cls.ClassId
                                                               where k.KindergartenId == value.KindergartenId
                                                               && q.CreateTime.Year == value.Year
                                                               && q.CreateTime.Month == g.Key
                                                               && cls.ClassId == value.ClassId
                                                               && clsQuestionInfoList.Contains(q.QuestionType)
                                                               group q by q.QuestionType into gn
                                                               select new
                                                               {
                                                                   type = gn.Key,
                                                                   typeCount = gn.Count()
                                                               }

                                                };


                        response.Attach = new
                        {
                            messageInfo = messageMonthInfo.ToList(),
                            questionInfo = questionMonthInfo.ToList(),
                        };
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
            return messageInfo(request);
        }
	}
}