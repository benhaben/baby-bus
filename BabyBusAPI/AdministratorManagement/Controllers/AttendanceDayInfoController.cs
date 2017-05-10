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
    public class AttendanceDayInfoController : BabyBusApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(AttendanceDayInfoController));
        //

        public class AttendanceRequest
        {
            public int Year { get; set; }
            public int KindergartenId { get; set; }
            public int Month { get; set; }
            public int Day { get; set; }
            public int PermissionType { set; get; }
            public int ClassId { set; get; }
        }



        private ApiResponser AttendanceInfo(AttendanceRequest value)
        {
            StartWatch();
            var response = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {//child

                    var classChildCount = from ad in db.AttendanceDetails
                                          join cd in db.Child
                                          on ad.ChildId equals cd.ChildId
                                          where cd.KindergartenId == value.KindergartenId
                                          && ad.CreateDate.Year == value.Year
                                          && ad.CreateDate.Month == value.Month
                                          && ad.CreateDate.Day == value.Day
                                          group ad by cd.ClassId into g
                                          select new
                                          {
                                              classId = g.Key,
                                              childCount = g.Count()
                                          };
                    
                    var attendanceCqInfo = from ad in db.AttendanceDetails
                                           join cd in db.Child
                                           on ad.ChildId equals cd.ChildId
                                           join cs in db.Classes
                                           on cd.ClassId equals cs.ClassId
                                           where cd.KindergartenId == value.KindergartenId
                                           && ad.CreateDate.Year == value.Year
                                           && ad.CreateDate.Month == value.Month
                                           && ad.CreateDate.Day == value.Day
                                           && ad.Status == 1
                                           && cs.ClassType != -1
                                           orderby cs.ClassType descending
                                           group ad by new { cs.ClassId, cs.ClassName } into g
                                           select new
                                           {
                                               classId = g.Key.ClassId,
                                               className = g.Key.ClassName,
                                               cqCount = g.Count()
                                           };
                    
                    response.Attach = new
                    {
                        classChildCount = classChildCount.ToList(),
                        attendanceCqInfo = attendanceCqInfo.ToList()
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

        private ApiResponser AttendanceInfo2(AttendanceRequest value)
        {
            StartWatch();
            var response = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {//child

                    var classChildCount = from ad in db.AttendanceDetails
                                          join cd in db.Child
                                          on ad.ChildId equals cd.ChildId
                                          where cd.KindergartenId == value.KindergartenId
                                          && cd.ClassId == value.ClassId
                                          && ad.CreateDate.Year == value.Year
                                          && ad.CreateDate.Month == value.Month
                                          && ad.CreateDate.Day == value.Day
                                          group ad by cd.ClassId into g
                                          select new
                                          {
                                              classId = g.Key,
                                              childCount = g.Count()
                                          };

                    var attendanceCqInfo = from ad in db.AttendanceDetails
                                           join cd in db.Child
                                           on ad.ChildId equals cd.ChildId
                                           join cs in db.Classes
                                           on cd.ClassId equals cs.ClassId
                                           where cd.KindergartenId == value.KindergartenId
                                           && cd.ClassId == value.ClassId
                                           && ad.CreateDate.Year == value.Year
                                           && ad.CreateDate.Month == value.Month
                                           && ad.CreateDate.Day == value.Day
                                           && ad.Status == 1
                                           orderby cs.ClassType descending
                                           group ad by new { cs.ClassId, cs.ClassName } into g
                                           select new
                                           {
                                               classId = g.Key.ClassId,
                                               className = g.Key.ClassName,
                                               cqCount = g.Count()
                                           };

                    response.Attach = new
                    {
                        classChildCount = classChildCount.ToList(),
                        attendanceCqInfo = attendanceCqInfo.ToList()
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

        // GET: /AttendanceDayInfo/
        [System.Web.Http.HttpPost]
        public ApiResponser Post(AttendanceRequest value)
        {
            //教师
            if(value.PermissionType == 2){
                return AttendanceInfo2(value);
            }
            return AttendanceInfo(value);
        }
	}
}