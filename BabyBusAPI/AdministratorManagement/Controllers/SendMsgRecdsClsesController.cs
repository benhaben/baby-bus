using BabyBus.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BabyBus.AutoModel;

namespace AdministratorManagement.Controllers
{
    public class SendMsgRecdsClsesController : BabyBusApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(SendMsgRecdsClsesController));
        public class SendMsgRecdsClses
        {
            public int Year { get; set; }
            public int Month { get; set; }
            public int KingdergartendId { get; set; }
            public int ClassType { get; set; }
            public int Type { get; set; }
            public int ClassId { get; set; }
        }

        private ApiResponser msgRecdsClsesInfo(SendMsgRecdsClses vaule)
        {
            StartWatch();
            var response = new ApiResponser(true);

            try
            {
                using (var db = new BabyBus_Entities())
                {
                    if (vaule.Type == 0)
                    {
                        var classInfo = from c in db.Classes
                                        where c.KindergartenId == vaule.KingdergartendId
                                        && c.Cancel == false
                                        && c.ClassType != -1
                                        && c.ClassType == vaule.ClassType
                                        orderby c.ClassType descending
                                        group c by new { c.ClassId, c.ClassName } into gb
                                        select new
                                        {
                                            className = gb.Key.ClassName,
                                            classId = gb.Key.ClassId
                                        };

                        //排除园区消息
                        List<int> clsNoticeTypeList = new List<int> { 1, 2, 6 };
                        var messageMonthInfo = from n in db.Notice
                                               where n.KindergartenId == vaule.KingdergartendId
                                               && n.CreateTime.Year == vaule.Year
                                               && n.CreateTime.Month == vaule.Month
                                               && n.ClassId != 0
                                               && clsNoticeTypeList.Contains(n.NoticeType)
                                               group n by n.ClassId into g
                                               select new
                                               {
                                                   classId = g.Key,
                                                   sumCount = g.Count(),
                                                   typeInfo = from nd in db.Notice
                                                              where nd.KindergartenId == vaule.KingdergartendId
                                                              && nd.CreateTime.Year == vaule.Year
                                                              && nd.CreateTime.Month == vaule.Month
                                                              && nd.ClassId == g.Key
                                                              group nd by nd.NoticeType into gnd
                                                              select new
                                                              {
                                                                  typeNo = gnd.Key,
                                                                  typeCount = gnd.Count(),
                                                              }
                                               };

                        //0(请假),1(教师提问),2(园长提问)
                        List<int> clsQuestionInfoList = new List<int> { 0, 1 };
                        var questionMonthInfo = from q in db.Question
                                                join cd in db.Child
                                                on q.ChildId equals cd.ChildId
                                                join k in db.Kindergarten
                                                on cd.KindergartenId equals k.KindergartenId
                                                join cs in db.Classes
                                                on cd.ClassId equals cs.ClassId
                                                where k.KindergartenId == vaule.KingdergartendId
                                                && cs.ClassType != -1
                                                && q.CreateTime.Year == vaule.Year
                                                && q.CreateTime.Month == vaule.Month
                                                && cs.ClassType == vaule.ClassType
                                                && clsQuestionInfoList.Contains(q.QuestionType)
                                                orderby cs.ClassType descending
                                                group cs by cs.ClassId into g
                                                select new
                                                {
                                                    classId = g.Key,
                                                    sumCount = g.Count(),
                                                    typeInfo = from q in db.Question
                                                               join cd in db.Child
                                                               on q.ChildId equals cd.ChildId
                                                               join k in db.Kindergarten
                                                               on cd.KindergartenId equals k.KindergartenId
                                                               join cs in db.Classes
                                                               on cd.ClassId equals cs.ClassId
                                                               where k.KindergartenId == vaule.KingdergartendId
                                                               && cs.ClassType != -1
                                                               && q.CreateTime.Year == vaule.Year
                                                               && q.CreateTime.Month == vaule.Month
                                                               && cs.ClassId == g.Key
                                                               && cs.ClassType == vaule.ClassType
                                                               group q by q.QuestionType into gn
                                                               select new
                                                               {
                                                                   type = gn.Key,
                                                                   typeCount = gn.Count()
                                                               }
                                                };


                        response.Attach = new
                        {
                            classInfo = classInfo.ToList(),
                            messageInfo = messageMonthInfo.ToList(),
                            questionMonthInfo = questionMonthInfo.ToList(),
                        };
                    }
                    else {
                        var classInfo = from c in db.Classes
                                        where c.KindergartenId == vaule.KingdergartendId
                                        && c.Cancel == false
                                        && c.ClassType != -1
                                        && c.ClassId == vaule.ClassId
                                        orderby c.ClassType descending
                                        group c by new { c.ClassId, c.ClassName } into gb
                                        select new
                                        {
                                            className = gb.Key.ClassName,
                                            classId = gb.Key.ClassId
                                        };

                        //排除园区消息
                        List<int> clsNoticeTypeList = new List<int> { 1, 2, 6 };
                        var messageMonthInfo = from n in db.Notice
                                               where n.KindergartenId == vaule.KingdergartendId
                                               && n.CreateTime.Year == vaule.Year
                                               && n.CreateTime.Month == vaule.Month
                                               && n.ClassId != 0
                                               && n.ClassId == vaule.ClassId
                                               && clsNoticeTypeList.Contains(n.NoticeType)
                                               group n by n.ClassId into g
                                               select new
                                               {
                                                   classId = g.Key,
                                                   sumCount = g.Count(),
                                                   typeInfo = from nd in db.Notice
                                                              where nd.KindergartenId == vaule.KingdergartendId
                                                              && nd.CreateTime.Year == vaule.Year
                                                              && nd.CreateTime.Month == vaule.Month
                                                              && nd.ClassId == g.Key
                                                              group nd by nd.NoticeType into gnd
                                                              select new
                                                              {
                                                                  typeNo = gnd.Key,
                                                                  typeCount = gnd.Count(),
                                                              }
                                               };

                        //0(请假),1(教师提问)
                        List<int> clsQuestionInfoList = new List<int> { 0, 1 };
                        var questionMonthInfo = from q in db.Question
                                                join cd in db.Child
                                                on q.ChildId equals cd.ChildId
                                                join k in db.Kindergarten
                                                on cd.KindergartenId equals k.KindergartenId
                                                join cs in db.Classes
                                                on cd.ClassId equals cs.ClassId
                                                where k.KindergartenId == vaule.KingdergartendId
                                                && cs.ClassType != -1
                                                && q.CreateTime.Year == vaule.Year
                                                && q.CreateTime.Month == vaule.Month
                                                && clsQuestionInfoList.Contains(q.QuestionType)
                                                && cd.ClassId == vaule.ClassId
                                                orderby cs.ClassType descending
                                                group cs by cs.ClassId into g
                                                select new
                                                {
                                                    classId = g.Key,
                                                    sumCount = g.Count(),
                                                    typeInfo = from q in db.Question
                                                               join cd in db.Child
                                                               on q.ChildId equals cd.ChildId
                                                               join k in db.Kindergarten
                                                               on cd.KindergartenId equals k.KindergartenId
                                                               join cs in db.Classes
                                                               on cd.ClassId equals cs.ClassId
                                                               where k.KindergartenId == vaule.KingdergartendId
                                                               && cs.ClassType != -1
                                                               && q.CreateTime.Year == vaule.Year
                                                               && q.CreateTime.Month == vaule.Month
                                                               && cs.ClassId == g.Key
                                                               group q by q.QuestionType into gn
                                                               select new
                                                               {
                                                                   type = gn.Key,
                                                                   typeCount = gn.Count()
                                                               }
                                                };


                        response.Attach = new
                        {
                            classInfo = classInfo.ToList(),
                            messageInfo = messageMonthInfo.ToList(),
                            questionMonthInfo = questionMonthInfo.ToList(),
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

        //查询出园区的消息
        private ApiResponser msgRecdsClsesInfo2(SendMsgRecdsClses vaule)
        {
            StartWatch();
            var response = new ApiResponser(true);

            try
            {
                using (var db = new BabyBus_Entities())
                {
                    //只查询园区消息
                    List<int> clsNoticeTypeList = new List<int> { 4, 5, 7 };
                    var messageMonthInfo = from n in db.Notice
                                           where n.KindergartenId == vaule.KingdergartendId
                                           && n.CreateTime.Year == vaule.Year
                                           && n.CreateTime.Month == vaule.Month
                                           && n.ClassId == 0
                                           && clsNoticeTypeList.Contains(n.NoticeType)
                                           group n by n.ClassId into g
                                           select new
                                           {
                                               classId = g.Key,
                                               sumCount = g.Count(),
                                               typeInfo = from nd in db.Notice
                                                          where nd.KindergartenId == vaule.KingdergartendId
                                                          && nd.CreateTime.Year == vaule.Year
                                                          && nd.CreateTime.Month == vaule.Month
                                                          && nd.ClassId == g.Key
                                                          group nd by nd.NoticeType into gnd
                                                          select new
                                                          {
                                                              typeNo = gnd.Key,
                                                              typeCount = gnd.Count(),
                                                          }
                                           };

                    //2(园长提问)
                    var questionMonthInfo = from q in db.Question
                                            join cd in db.Child
                                            on q.ChildId equals cd.ChildId
                                            join k in db.Kindergarten
                                            on cd.KindergartenId equals k.KindergartenId
                                            where k.KindergartenId == vaule.KingdergartendId
                                            && q.CreateTime.Year == vaule.Year
                                            && q.CreateTime.Month == vaule.Month
                                            && q.QuestionType == 2
                                            select q;


                    response.Attach = new
                    {
                        messageInfo = messageMonthInfo.ToList(),
                        questionMonthInfoCount = questionMonthInfo.Count(),
                    };

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
        public ApiResponser Post(SendMsgRecdsClses vaule)
        {
            if (vaule.ClassType != -1){
                return msgRecdsClsesInfo(vaule);
            }else {
                return msgRecdsClsesInfo2(vaule);
            }
            
        }
	}
}