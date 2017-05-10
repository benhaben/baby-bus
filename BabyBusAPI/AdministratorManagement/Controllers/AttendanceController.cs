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
    public class AttendanceController : BabyBusApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(AttendanceController));
        public AttendanceController() { 
        
        }

        public class AttendanceRequest
        {
            public int Year { get; set; }
            public int KindergartenId { get; set; }
            public int Type { get; set; }
            public int ClassId { get; set; }
            public int isExprotExcel { set; get; }
        }

        private ApiResponser AttendanceMasterInfo(AttendanceRequest value)
        {
            StartWatch();
            var response = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    if (value.Type == 0)
                    {
                        //child
                        var childInfo = from c in db.Child
                                        join cs in db.Classes
                                        on c.ClassId equals cs.ClassId
                                        where c.KindergartenId == value.KindergartenId
                                        && cs.ClassName != "删除"
                                        && c.Cancel == false
                                        && cs.Cancel == false
                                        select c;

                        var attendanceMasterInfo = from am in db.AttendanceMasters
                                                   where am.KindergartenId == value.KindergartenId
                                                   && am.CreateDate.Year == value.Year
                                                   group am by am.CreateDate.Month into g
                                                   select new { key = g.Key, sum = g.Sum(am => am.Attence) };
                        response.Attach = new
                        {
                            ChildCount = childInfo.Count(),
                            attendanceMasterInfo = attendanceMasterInfo.ToList(),
                        };
                    }
                    else {
                        //child
                        var childInfo = from c in db.Child
                                        where c.KindergartenId == value.KindergartenId
                                        && c.ClassId == value.ClassId
                                        select c;

                        var attendanceMasterInfo = from am in db.AttendanceMasters
                                                   where am.KindergartenId == value.KindergartenId
                                                   && am.CreateDate.Year == value.Year
                                                   && am.ClassId == value.ClassId
                                                   group am by am.CreateDate.Month into g
                                                   select new { key = g.Key, sum = g.Sum(am => am.Attence) };
                        response.Attach = new
                        {
                            ChildCount = childInfo.Count(),
                            attendanceMasterInfo = attendanceMasterInfo.ToList(),
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
        public ApiResponser Post(AttendanceRequest value)
        {
            if (value.isExprotExcel == -1)
            {
            }
            return AttendanceMasterInfo(value);
            //return ExamineInfo(value);
        }

	}
}
