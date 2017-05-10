using BabyBus.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BabyBus.AutoModel;

namespace AdministratorManagement.Controllers
{
    public class AttendanceClassesController : BabyBusApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(AttendanceClassesController));
        public class AttendanceClasses
        {
            public int Year { get; set; }
            public int Month { get; set; }
            public int KingdergartendId { get; set; }
            public int Type { get; set; }
            public int ClassId { set; get; }
            public int ClassType { set; get; }

        }
        private ApiResponser UserAttendanceClass(AttendanceClasses values)
        {
            StartWatch();
            var responser = new ApiResponser(true);
            string hour = DateTime.Now.Hour.ToString();

            try
            {
                var db = new BabyBus_Entities();

                if (values.Type == 0)
                {
                    var attendanceClass = from c in db.Classes
                                          where c.KindergartenId == values.KingdergartendId
                                          && c.Cancel == false
                                          && c.ClassType != -1
                                          && c.ClassType == values.ClassType
                                          orderby c.ClassType descending
                                          group c by new { c.ClassId, c.ClassName } into gb
                                          select new { gb.Key.ClassName, gb.Key.ClassId };

                    //HttpContext.Current.Request.ServerVariables.Add("jjj",attendanceClass.ToString());

                    var childClass = from cd in db.Child
                                     where cd.KindergartenId == values.KingdergartendId
                                     && cd.Cancel == false
                                     group cd by cd.ClassId into gb
                                     select new { key = gb.Key, sum = gb.Count()};

                    var actualAttendance = from aa in db.AttendanceMasters
                                           where aa.KindergartenId == values.KingdergartendId 
                                           && aa.CreateDate.Year == values.Year 
                                           && aa.CreateDate.Month == values.Month
                                           group aa by aa.ClassId into gb
                                           select new
                                           {
                                               key = gb.Key,
                                               sAtten = gb.Sum(aa => aa.Attence),
                                               qAtten = gb.Sum(aa => (aa.Total - aa.Attence))
                                           };
                    var todayAttenInfo = from ad in db.AttendanceDetails
                                         join am in db.AttendanceMasters
                                         on ad.MasterId equals am.MasterId
                                         where am.KindergartenId == values.KingdergartendId
                                         && am.CreateDate.Year == values.Year
                                         && am.CreateDate.Month == values.Month
                                         && am.CreateDate.Day == DateTime.Today.Day
                                         && ad.CreateDate.Day == DateTime.Today.Day
                                         group am by am.ClassId into gb
                                         select new
                                         {
                                             classId = gb.Key,
                                             attendCount = gb.Count()
                                         };

                    responser.Attach = new
                    {
                        AttendanceClass = attendanceClass.ToList(),
                        ChildClass = childClass.ToList(),
                        ActualAttendance = actualAttendance.ToList(),
                        TodayAttenInfo = todayAttenInfo.ToList(),
                    };
                }
                else 
                {
                    var attendanceClass = from c in db.Classes
                                          where c.KindergartenId == values.KingdergartendId
                                          && c.ClassId == values.ClassId
                                          && c.ClassType != -1
                                          orderby c.ClassType descending
                                          group c by new { c.ClassId, c.ClassName } into gb
                                          select new { gb.Key.ClassName, gb.Key.ClassId };


                    var childClass = from cd in db.Classes
                                     where cd.KindergartenId == values.KingdergartendId
                                     && cd.ClassId == values.ClassId
                                     && cd.ClassType != -1
                                     orderby cd.ClassType descending
                                     group cd by cd.ClassId into gb
                                     select new { key = gb.Key, sum = gb.Sum(cd => cd.ClassCount) };

                    var actualAttendance = from aa in db.AttendanceMasters
                                           where aa.KindergartenId == values.KingdergartendId
                                           && aa.CreateDate.Year == values.Year 
                                           && aa.CreateDate.Month == values.Month
                                           && aa.ClassId == values.ClassId
                                           group aa by aa.ClassId into gb
                                           select new
                                           {
                                               key = gb.Key,
                                               sAtten = gb.Sum(aa => aa.Attence),
                                               qAtten = gb.Sum(aa => (aa.Total - aa.Attence))
                                           };

                    var todayAttenInfo = from ad in db.AttendanceDetails
                                         join am in db.AttendanceMasters
                                         on ad.MasterId equals am.MasterId
                                         where am.KindergartenId == values.KingdergartendId
                                         && am.ClassId == values.ClassId
                                         && am.CreateDate.Year == values.Year
                                         && am.CreateDate.Month == values.Month
                                         && am.CreateDate.Day == DateTime.Today.Day
                                         && ad.CreateDate.Day == DateTime.Today.Day
                                         group am by am.ClassId into gb
                                         select new
                                         {
                                             classId = gb.Key,
                                             attendCount = gb.Count()
                                         };

                    responser.Attach = new
                    {
                        AttendanceClass = attendanceClass.ToList(),
                        ChildClass = childClass.ToList(),
                        ActualAttendance = actualAttendance.ToList(),
                        TodayAttenInfo = todayAttenInfo.ToList(),
                    };
                }

            }
            catch (Exception ex)
            {
                responser.Status = false;
                responser.Message = ex.Message;
                Log.Fatal(ex.Message, ex);
                return responser;
            }
            EndWatch();
            return responser;
        }

        [System.Web.Http.HttpPost]
        public ApiResponser Post(AttendanceClasses values)
        {
            return UserAttendanceClass(values);
        }
    }
}