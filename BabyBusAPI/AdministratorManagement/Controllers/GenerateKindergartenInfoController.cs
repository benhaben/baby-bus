using System;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using BabyBus.AutoModel;
using BabyBus.Core.Helper;
using System.Web.SessionState;

namespace AdministratorManagement.Controllers
{
    public class GenerateKindergartenInfoController : BabyBusApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(GenerateKindergartenInfoController));

        public enum PermissionType 
        {
            Administrator = 0,
            SuperPresident = 4,
            President = 3,
            Teacher = 2,
            Parent = 1
        }

        public class GenerateKindergartenInfoType 
        {
            public int kindergartenId { set; get; }
            public PermissionType permissionType { set; get; }
            public int userId { set; get; }
        }
        private ApiResponser geneKindInfoForAdmin()
        {
            StartWatch("in to Kindergartens");

            var response = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var kindergartens = (from i in db.Kindergarten
                                         orderby i.KindergartenId
                                         where i.Cancel == false
                                         select new
                                         {
                                             KindergartenId = i.KindergartenId,
                                             KindergartenName = i.KindergartenName
                                         }).Distinct();
                    response.Attach = new
                    {
                        Kindergartens = kindergartens.ToList()
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

        private ApiResponser geneKindInfoForSuperPresident(GenerateKindergartenInfoType request)
        {
            StartWatch("in to Kindergartens");

            var response = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var kindergartens = (from k in db.Kindergarten
                                        join uk in db.UserKindergartenRealations
                                        on k.KindergartenId equals uk.KindergartenId
                                        where uk.UserId == request.userId
                                        && k.Cancel == false
                                        orderby uk.KindergartenId
                                        select new
                                        {
                                            KindergartenId = k.KindergartenId,
                                            KindergartenName = k.KindergartenName
                                        }).Distinct();
                    response.Attach = new
                    {
                        Kindergartens = kindergartens.ToList()
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
        }// geneKindInfoForSuperPresident end

        private ApiResponser geneKindInfoForPresidentAndTeacher(GenerateKindergartenInfoType request)
        {
            StartWatch("in to Kindergartens");

            var response = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var kindergartens = from k in db.Kindergarten
                                        where k.KindergartenId == request.kindergartenId
                                        && k.Cancel == false
                                        orderby k.KindergartenId
                                        select new
                                        {
                                            KindergartenId = k.KindergartenId,
                                            KindergartenName = k.KindergartenName
                                        };
                    response.Attach = new
                    {
                        Kindergartens = kindergartens.ToList()
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
        }// geneKindInfoForPresidentAndTeacher end




        [System.Web.Http.HttpPost]
        public ApiResponser Post(GenerateKindergartenInfoType request)
        {
            if(request.permissionType == PermissionType.Administrator)
            {
                return this.geneKindInfoForAdmin();
            }else if(request.permissionType == PermissionType.SuperPresident)
            {
                return this.geneKindInfoForSuperPresident(request);
            }
            else if (request.permissionType == PermissionType.President || request.permissionType == PermissionType.Teacher)
            {
                return this.geneKindInfoForPresidentAndTeacher(request);
            }
            else 
            {
                return null;
            }
        }// Post end
	}
}