using BabyBus.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BabyBus.AutoModel;

namespace AdministratorManagement.Controllers
{
    public class AttendancePlanController : BabyBusApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(AttendancePlanController));
        //
        // GET: /AttendancePlane/
        public int KindergartenId { get; set; }
        private ApiResponser AttendancePlan()
        {
            StartWatch();
            var response = new ApiResponser(true);
            try { 
            using (var db = new BabyBus_Entities()){
                var attendancePlanChilds = (from c in db.Child orderby c.KindergartenId == 13 select c).Count();
               
                response.Attach = new 
                {
                    AttendanceCount=attendancePlanChilds
                };
            
            }
                }catch(Exception ex){
                    response.Status = false;
                    response.Message = ex.Message;
                    Log.Fatal(ex.Message, ex);
                    return response;
                }
            EndWatch();
            return response;
        }

       
    [System.Web.Http.HttpPost]
        public ApiResponser Post()
        {
            return AttendancePlan();
        }
	}
}