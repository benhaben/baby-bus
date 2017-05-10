using System;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using BabyBus.AutoModel;
using BabyBus.Core.Helper;
using System.Web.SessionState;



namespace AdministratorManagement.Controllers
{
    //[Authorize]
    public class ManagementController : BabyBusApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(ManagementController));
        public ManagementController()
        {
        }

        public enum ManagementRequestType
        {
            Child = 0,
            Kindergarten = 1,
            Count = 2,
            Classes = 3,
            ChildTeacher = 4,
            EditChild = 5,
        }
        public class ManagementRequest
        {
            public ManagementRequestType ManagementRequestType { get; set; }
            public int KindergartenId { get; set; }

            public string KindergartenName { get; set; }

            public int ClassId { get; set; }

            public string ClassName { get; set; }

            public int ChildId { get; set; }

            public string ChildName { get; set; }
            public int Role { get; set; }
        }




        [System.Web.Http.HttpPost]
        //Post: api/NoticeHomework
        public ApiResponser Post(ManagementRequest value)
        {
            if (value.ManagementRequestType == ManagementRequestType.Count)
            {
                return UserCount();
            }
            else if (value.ManagementRequestType == ManagementRequestType.EditChild)
            {
                return ChangeChildKindergartenAndClassByChildId(value);
            }
            return null;
        }
        private ApiResponser ChangeChildKindergartenAndClassByChildId(ManagementRequest value)
        {
            StartWatch();
            var response = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    db.sp_changeChildKindergartenAndClassByChildId(value.ChildId, value.KindergartenName,
                        value.ClassName);
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
        }

        private ApiResponser UserCount()
        {
            StartWatch();

            var response = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    //Mapper.DynamicMap(model, notice);
                    var teachers = from i in db.TeacherInUseSummary
                                   select i;
                    var parents = from i in db.ChildParentInformationViewWithIds
                                  orderby i.KindergartenId, i.ClassId
                                  select i;
                    var kindergartens = (from i in db.TeacherInUseSummary
                                         orderby i.KindergartenId
                                         select new { KindergartenId = i.KindergartenId, KindergartenName = i.KindergartenName }).Distinct();

                    response.Attach = new
                    {
                        TeachersCount = teachers.Count(),
                        ParentsCount = parents.Count(),
                        KindergartensCount = kindergartens.Count()
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
        }
    }
}