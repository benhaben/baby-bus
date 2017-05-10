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
    public class AttendanceClsDetailController : BabyBusApiController
    {
        //
        // GET: /AttendanceClsDetail/
        public AttendanceClsDetailController() { }

        public class childCountInfoCls{
            public int childId{set;get;}
            public string childName{set;get;}
            public int qqCount{set;get;}
            public string phone{set;get;}
        }


        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(ManagementController));

        public class AttendanceClsDetailRequest{
            public int calssId { set; get; }
            public int year { set; get; }
            public int month { set; get; }
            public int KindergartenId { get; set; }
        }

        private ApiResponser AttendanceClsDetailInfo(AttendanceClsDetailRequest value)
        {
            StartWatch();
            var response = new ApiResponser(true);

            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var allChildAndParentInfos = from ad in db.AttendanceDetails
                                                 join cd in db.Child
                                                 on ad.ChildId equals cd.ChildId
                                                 where ad.CreateDate.Year == value.year
                                                 && ad.CreateDate.Month == value.month
                                                 && cd.ClassId == value.calssId
                                                 group cd by new { cd.ChildId, cd.ChildName } into g
                                                 select new
                                                 {
                                                     childId = g.Key.ChildId,
                                                     childName = g.Key.ChildName,
                                                     parentInfo = from u in db.Users
                                                                  join p in db.ParentChildRelation
                                                                  on u.UserId equals p.UserId
                                                                  join c in db.Child
                                                                  on p.ChildId equals g.Key.ChildId
                                                                  select new
                                                                  {
                                                                      u.LoginName,
                                                                      u.UserId,
                                                                      u.Phone
                                                                  },
                                                     qqCount = (from ad in db.AttendanceDetails
                                                                where ad.ChildId == g.Key.ChildId
                                                                && ad.CreateDate.Year == value.year
                                                                && ad.CreateDate.Month == value.month
                                                                && ad.Status == 0
                                                                select ad).Count(),
                                                 };
                    //所有小孩出勤总汇
                    var allChildsAtDetails = from ad in db.AttendanceDetails
                                             join cd in db.Child
                                             on ad.ChildId equals cd.ChildId
                                             where ad.CreateDate.Year == value.year
                                             && ad.CreateDate.Month == value.month
                                             && cd.ClassId == value.calssId
                                             group ad by new { cd.ChildId, cd.ChildName } into g
                                             select new
                                             {
                                                 childId = g.Key.ChildId,
                                                 childName = g.Key.ChildName,
                                                 attendenceDetials = from ad in db.AttendanceDetails
                                                                     where ad.ChildId == g.Key.ChildId
                                                                     && ad.CreateDate.Year == value.year
                                                                     && ad.CreateDate.Month == value.month
                                                                     select new
                                                                     {
                                                                         attendenceDetialInfo = ad,
                                                                         month = ad.CreateDate.Month,
                                                                         day = ad.CreateDate.Day
                                                                     },
                                                 
                                             };

                    //测试调用存储过程
                    //var testData = db.UP_Attendance_GenerateExlsInfoForClassChild(value.year, value.month, value.KindergartenId, value.calssId);

                    response.Attach = new
                    {
                        allChildsAtDetails = allChildsAtDetails.ToList(),
                        allChildAndParentInfos = allChildAndParentInfos.ToList()
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
        public ApiResponser Post(AttendanceClsDetailRequest  value)
        {
            return AttendanceClsDetailInfo(value);
            //return null;
        }
	}
}