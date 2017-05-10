using BabyBus.AutoModel;
using BabyBus.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace AdministratorManagement.Controllers
{
    public class SendMessageStatisticsController : BabyBusApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(SendMessageStatisticsController));

        public class StatisticsRequest
        {
            public int teacherId { get; set; }
            public int year { get; set; }
            public int month { get; set; }
            public int kindergartenId { set; get; }
        }

        private ApiResponser noticeStatisticsByTeacher(StatisticsRequest value)
        {
            StartWatch("");

            var response = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    /*var noticeStatistics = from n in db.Notice
                                           where n.UserId == value.teacherId
                                           && n.CreateTime.Year == value.year
                                           group n by n.CreateTime.Month into g
                                           orderby g.Key
                                           select new{
                                               month=g.Key,
                                               noticeCount = g.Count()
                                           };*/

                    var noticeStatistics = from n in db.Notice
                                           join u in db.Users
                                           on n.UserId equals u.UserId
                                           where n.KindergartenId == value.kindergartenId
                                           && n.CreateTime.Year == value.year
                                           && n.CreateTime.Month == value.month
                                           && u.Cancel == false
                                           && u.RealName != ""
                                           group n by new { n.UserId ,u.RealName} into g
                                           orderby g.Count() descending
                                           select new
                                           {
                                               userId = g.Key.UserId,
                                               realName = g.Key.RealName,
                                               noticeCount = g.Count(),
                                               clsName = from cs in db.Classes
                                                         join u2 in db.Users
                                                         on cs.ClassId equals u2.ClassId
                                                         where u2.UserId == g.Key.UserId
                                                         select cs.ClassName,
                                           };
                    
                    response.Attach = new
                    {
                        noticeStatistics = noticeStatistics.ToList(),
                    };
                }
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
        }// presidentInfo end





        [System.Web.Http.HttpPost]
        public ApiResponser post(StatisticsRequest value)
        {
            return noticeStatisticsByTeacher(value);
        }//post End
    }
}