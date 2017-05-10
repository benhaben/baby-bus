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
    public class PhyExaminationChildInfoController : ApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(PhyExaminationChildInfoController));
        //
        // GET: /PhyExaminationChildInfo/

        public enum RequestType
        {
            medicalChildInfo = 0,
            medicalNomal = 1,
            medicalReport = 2,
        }

        public class PhyExaminationChildInfo
        {
            public RequestType requestType { get; set; }

            public int ChildId { get; set; }

            public DateTimeOffset TestTime { get; set; }

            public float Height { get; set; }

            public float Age { get; set; }

            public int Gender { get; set; }

            public int PhyExaminationPlanId { get; set; }
        }

        private ApiResponser phyMedicalInfo(PhyExaminationChildInfo value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var phyMedicalChildInfo = from c in db.Child
                                              join cl in db.Classes on c.ClassId equals cl.ClassId
                                              join k in db.Kindergarten on c.KindergartenId equals k.KindergartenId
                                              where c.ChildId == value.ChildId
                                              select new
                                              {
                                                  childNames = c.ChildName,
                                                  clsNames = cl.ClassName,
                                                  kinderNames = k.KindergartenName,
                                                  genders = c.Gender,
                                                  images = c.ImageName,
                                              };
                    responser.Attach = new
                    {
                        ChildPhyMedicalInfo = phyMedicalChildInfo.ToList(),
                    };
                }
            }
            catch (Exception e)
            {
                responser.Status = false;
                responser.Message = e.Message;
                Log.Fatal(e.Message, e);
                return responser;
            }
            return responser;
        }

        private ApiResponser phyMedicalNormalInfo(PhyExaminationChildInfo value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var medicalNomal = from i in db.IndividualIndexDistributions
                                       where i.Age == value.Age && i.Gender == value.Gender
                                       select i;
                    var weightForHeight = from h in db.HeightForStandardWeights
                                          where value.Height >= h.HeightOffline && value.Height <= h.HeightOnline
                                          select h;
                    responser.Attach = new
                    {
                        MedicalNormalInfo = medicalNomal.ToList(),
                        WeightNormalInfo = weightForHeight.ToList(),
                    };
                }
            }
            catch (Exception e)
            {
                responser.Status = false;
                responser.Message = e.Message;
                Log.Fatal(e.Message, e);
                return responser;
            }
            return responser;
        }

        private ApiResponser phyMedicalReport(PhyExaminationChildInfo value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var childPhyMedicalResultInfo = from pr in db.PhysicalExaminationResults
                                                    join p in db.PhysicalExaminationPlanes on pr.PhysicalExaminationPlaneId equals p.PhysicalExaminationPlaneId
                                                    where pr.ChildId == value.ChildId && pr.PhysicalExaminationPlaneId == value.PhyExaminationPlanId
                                                    select new
                                                    {
                                                        p.PlanTitle,
                                                        pr,
                                                        years = pr.TestTime.Year,
                                                        months = pr.TestTime.Month,
                                                        days = pr.TestTime.Day,
                                                    };
                    responser.Attach = new
                    {
                        ChildPhyExaminationReport = childPhyMedicalResultInfo.ToList(),
                    };
                }
            }
            catch (Exception e)
            {
                responser.Status = false;
                responser.Message = e.Message;
                Log.Fatal(e.Message, e);
                return responser;
            }
            return responser;
        }

        [System.Web.Http.HttpPost]
        public ApiResponser post(PhyExaminationChildInfo value)
        {
            if (value.requestType == RequestType.medicalChildInfo)
            {
                return phyMedicalInfo(value);
            } if (value.requestType == RequestType.medicalNomal)
            {
                return phyMedicalNormalInfo(value);
            }
            if (value.requestType == RequestType.medicalReport)
            {
                return phyMedicalReport(value);
            }
            return null;
        }
    }
}