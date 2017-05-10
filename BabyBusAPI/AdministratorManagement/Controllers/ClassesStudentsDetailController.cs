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
    public class ClassesStudentsDetailController : BabyBusApiController
    {
        public ClassesStudentsDetailController() { }

        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(ManagementController));

        public class StudentDetailInfoRequest
        {
            public int childId { set; get; }
            public int year { set; get; }
            public int month { set; get; }
        }

        private ApiResponser studentDetailInfo(StudentDetailInfoRequest value)
        {
            StartWatch();
            var response = new ApiResponser(true);

            try
            {
                using (var db = new BabyBus_Entities())
                {


                    //classInfo
                    var childInfo = from c in db.Child
                                    where c.ChildId == value.childId
                                    select new
                                    {
                                        childInfo = c,
                                        className = from cls in db.Classes
                                                    where cls.ClassId == c.ClassId
                                                    select cls.ClassName,
                                        kindergartenName = from k in db.Kindergarten
                                                           where k.KindergartenId == c.KindergartenId
                                                           select k.KindergartenName
                                    };

                    var attendenceDetial = from ad in db.AttendanceDetails
                                           where ad.ChildId == value.childId
                                           && ad.CreateDate.Year == value.year
                                           && ad.CreateDate.Month == value.month
                                           select new { 
                                                attendenceDetialInfo = ad,
                                                month = ad.CreateDate.Month,
                                                day = ad.CreateDate.Day
                                           };

                    var parent = from u in db.Users
                                 join p in db.ParentChildRelation
                                 on u.UserId equals p.UserId
                                 join c in db.Child
                                 on p.ChildId equals c.ChildId
                                 where c.ChildId == value.childId
                                 select new
                                 {
                                     u.RealName,
                                     u.LoginName,
                                     u.UserId,
                                     u.Phone
                                 };

                    var teacher = from u in db.Users
                                  join c in db.Classes
                                  on u.ClassId equals c.ClassId
                                  join cd in db.Child
                                  on c.ClassId equals cd.ClassId
                                  where cd.ChildId == value.childId
                                  && u.RoleType == 2
                                  select new
                                  {
                                      u.RealName,
                                      u.LoginName,
                                      u.UserId,
                                      u.Phone
                                  };

                    response.Attach = new
                    {
                        childDetailInfo = childInfo.ToList(),
                        attendenceDetailInfo = attendenceDetial.ToList(),
                        Parent = parent.ToList(),
                        Teacher = teacher.ToList(),
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
        public ApiResponser Post(StudentDetailInfoRequest value)
        {
            return studentDetailInfo(value);
        }
    }
}
