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
    public class AttendanceDayDetailInfoController : BabyBusApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(AttendanceDayDetailInfoController));
        //
        // GET: /AttendanceDayDetailInfo/
        public class AttendanceRequest
        {
            public int Year { get; set; }
            public int ClassId { get; set; }
            public int Month { get; set; }
            public int Day { get; set; }
        }



        private ApiResponser AttendanceClsDetailInfo(AttendanceRequest value)
        {
            StartWatch();
            var response = new ApiResponser(true);

            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var childAttInfo = from ad in db.AttendanceDetails
                                       join cd in db.Child
                                       on ad.ChildId equals cd.ChildId
                                       where cd.ClassId == value.ClassId
                                       && ad.CreateDate.Year == value.Year
                                       && ad.CreateDate.Month == value.Month
                                       && ad.CreateDate.Day == value.Day
                                       group ad by new { cd.ChildId, cd.ChildName, cd.ImageName} into g
                                       select new
                                       {
                                           childId = g.Key.ChildId,
                                           childName = g.Key.ChildName,
                                           childImgName = g.Key.ImageName,
                                           adInfo = from ad1 in db.AttendanceDetails
                                                    where ad1.ChildId == g.Key.ChildId
                                                    && ad1.CreateDate.Year == value.Year
                                                    && ad1.CreateDate.Month == value.Month
                                                    && ad1.CreateDate.Day == value.Day
                                                    select ad1,
                                       };
                    
                    response.Attach = new
                    {
                        childAttInfo = childAttInfo.ToList()
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
        public ApiResponser Post(AttendanceRequest value)
        {
            return AttendanceClsDetailInfo(value);
        }
	}
}