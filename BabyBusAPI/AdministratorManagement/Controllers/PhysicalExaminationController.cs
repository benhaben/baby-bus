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
    public class PhysicalExaminationController : ApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(PhysicalExaminationController));

        public enum DataType
        {
            weightHeightDetail = 1,
            individuvalStInfo = 2,
            sendAddIndStDetail = 3,
            planPhyExaminaition = 4,
            planPhyExaminationInfo = 5,
            planPhyChildInfo = 6,
            displayNormal = 7
            
        }
        public class PhysicalExamination
        {
            public DataType DataType { get; set; }
            public int SelectInfo { get; set; }
            public float SelectAge { get; set; }
            public string SendAddIndividualSt { get; set; }
            public float SelectAddAge { get; set; }
            public int SendAddGender { get; set; }
            public int KindergardenId { get; set; }
            public DateTime PlanTime { get; set; }
            public string Description { get; set; }
            public int UserId { get; set; }
            public string PlanTitle{get;set;}
            public int Year { get; set; }
            public int PlanPhyExmamintionId { get; set; }
        }

        private ApiResponser weightHeightDetail(PhysicalExamination value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    if (value.SelectInfo == -1) {
                        var heightWeightDetail = from h in db.HeightForStandardWeights
                                                 group h by h.Gender into gb
                                                 select new
                                                 {
                                                     key = gb.Key,
                                                     detailInfo = from hs in db.HeightForStandardWeights
                                                                  where hs.Gender == gb.Key
                                                                  orderby hs.HeightOffline
                                                                  select hs
                                                 };
                        responser.Attach = new { 
                            HeightWeightDetail = heightWeightDetail.ToList(),
                        };
                    }
                    else
                    {
                        var heightWeightDetail = from h in db.HeightForStandardWeights
                                                 where h.Gender == value.SelectInfo
                                                 group h by h.Gender into gb
                                                 select new
                                                 {
                                                     key = gb.Key,
                                                     detailInfo = from hs in db.HeightForStandardWeights
                                                                  where hs.Gender == gb.Key
                                                                  orderby hs.HeightOffline
                                                                  select hs
                                                 };
                        responser.Attach = new
                        {
                            HeightWeightDetail = heightWeightDetail.ToList(),
                        };
                    }
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

        private ApiResponser individualStInfo(PhysicalExamination value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    if (value.SelectAge == -1) { 
                        var individualSt = from i in db.IndividualIndexDistributions
                                           group i by i.Age into gb
                                           select new
                                           {
                                               age = gb.Key,
                                               individualGender = from i in db.IndividualIndexDistributions
                                                               where i.Age == gb.Key
                                                               group i by i.Gender into gbg
                                                               select new
                                                               {
                                                                   gender = gbg.Key,
                                                                   individualStaDetail = from ins in db.IndividualIndexDistributions
                                                                                         where ins.Gender == gbg.Key 
                                                                                            && ins.Age == gb.Key
                                                                                         select ins
                                                               },
                                           };
                        responser.Attach = new
                        {
                            IndividualSt = individualSt.ToList(),
                        };
                    }
                    else
                    {
                        var individualSt = from i in db.IndividualIndexDistributions
                                           where i.Age == value.SelectAge
                                           group i by i.Age into gb
                                           select new
                                           {
                                               age = gb.Key,
                                               individualGender = from i in db.IndividualIndexDistributions
                                                                  where i.Age == gb.Key
                                                                  group i by i.Gender into gbg
                                                                  select new
                                                                  {
                                                                      gender = gbg.Key,
                                                                      individualStaDetail = from ins in db.IndividualIndexDistributions
                                                                                            where ins.Gender == gbg.Key && ins.Age == gb.Key
                                                                                            select ins
                                                                  },
                                           };
                        responser.Attach = new
                        {
                            IndividualSt = individualSt.ToList(),
                        };
                    }
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

        private ApiResponser addIndividualStDetail(PhysicalExamination value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    string [] individualLineData = value.SendAddIndividualSt.Split(new Char [] {';'});
                   
                    for (var i = 0; i < individualLineData.Length; i++)
                    {
                        var individualDetaiData = individualLineData[i];
                        if (individualDetaiData == "")
                        {
                            continue;
                        }
                        IndividualIndexDistribution individualInsertDetail = new IndividualIndexDistribution();
                        individualInsertDetail.CreateTime = DateTime.Now;
                        individualInsertDetail.Age = value.SelectAddAge;
                        individualInsertDetail.Gender = value.SendAddGender;

                        string[] individualAddDetailData = individualDetaiData.Split(new Char[] { ',' });
                        for (var d = 0; d < individualAddDetailData.Length; d++)
                        {
                            var individualDetailString = individualAddDetailData[d];
                            if (individualDetailString == "")
                            {
                                continue;
                            }
                            if (d == 0)
                            {
                                var individualDetailFlt = int.Parse(individualDetailString);
                                individualInsertDetail.PhysicalExaEIems = individualDetailFlt;
                            }else if(d == 1){
                                var individualDetailFlt = float.Parse(individualDetailString);
                                individualInsertDetail.OnePointsLower = individualDetailFlt;
                            }
                            else if (d == 2)
                            {
                                var individualDetailFlt = float.Parse(individualDetailString);
                                individualInsertDetail.OnePointsOnLine = individualDetailFlt;
                            }
                            else if (d == 3)
                            {
                                var individualDetailFlt = float.Parse(individualDetailString);
                                individualInsertDetail.TwoPointsLower = individualDetailFlt;
                            }else if( d == 4){
                                var individualDetailFlt = float.Parse(individualDetailString);
                                individualInsertDetail.TwoPointsOnLine = individualDetailFlt;
                            }
                            else if (d == 5)
                            {
                                var individualDetailFlt = float.Parse(individualDetailString);
                                individualInsertDetail.ThreePointsLower = individualDetailFlt;
                            }
                            else if (d == 6)
                            {
                                var individualDetailFlt = float.Parse(individualDetailString);
                                individualInsertDetail.ThreePointsOnLine = individualDetailFlt;
                            }
                            else if (d == 7)
                            {
                                var individualDetailFlt = float.Parse(individualDetailString);
                                individualInsertDetail.FourPointsLower = individualDetailFlt;
                            }
                            else if (d == 8)
                            {
                                var individualDetailFlt = float.Parse(individualDetailString);
                                individualInsertDetail.FourPointsOnLine = individualDetailFlt;
                            }
                            else if (d == 9)
                            {
                                var individualDetailFlt = float.Parse(individualDetailString);
                                individualInsertDetail.FivePointsLower = individualDetailFlt;
                            }
                            else if (d == 10)
                            {
                                var individualDetailFlt = int.Parse(individualDetailString);
                                individualInsertDetail.FivePointsOnLine = individualDetailFlt;
                            }
                        }
                        db.IndividualIndexDistributions.Add(individualInsertDetail);
                    }
                    db.SaveChanges();
                    responser.Attach = new
                    {
                        Status = 0,
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

        private ApiResponser planPhyExaminationInfo(PhysicalExamination value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var planInfo = new PhysicalExaminationPlane
                    {
                        KindergartenId = value.KindergardenId,
                        PlaneTime = value.PlanTime,
                        UserId = value.UserId,
                        CreateTime = DateTime.Now,
                        Description = value.Description,
                        PlanTitle = value.PlanTitle
                    };
                    db.PhysicalExaminationPlanes.Add(planInfo);
                    db.SaveChanges();
                    responser.Attach = new
                    {
                        Status = 0,
                        PhyPlanExaminationId = planInfo.PhysicalExaminationPlaneId,
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

        private ApiResponser planPhyExaDataInfo(PhysicalExamination value){
            var responser = new ApiResponser(true);
            try{
                using(var db = new BabyBus_Entities()){
                    var planPhyInfo = from p in db.PhysicalExaminationPlanes
                                      where p.KindergartenId == value.KindergardenId && p.PlaneTime.Year == value.Year
                                      orderby p.PlaneTime descending
                                      select new{
                                          p,
                                          years = p.PlaneTime.Year,
                                          months = p.PlaneTime.Month,
                                          days = p.PlaneTime.Day,
                                      };
                    responser.Attach = new
                    {
                        PlanPhyInfo = planPhyInfo.ToList(),
                    };
                }
                }catch(Exception e){
                    responser.Status = false;
                    responser.Message = e.Message;
                    Log.Fatal(e.Message,e);
                    return responser;
                }
            return responser;
        }

        private ApiResponser planPhyChildInfo(PhysicalExamination value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var classAndChildInfo = from cl in db.Classes
                        where cl.KindergartenId == value.KindergardenId 
                        && cl.Cancel == false
                        && cl.ClassType != -1
                        orderby cl.ClassType descending
                        select new
                        {
                            cl.ClassId,
                            cl.ClassName,
                            childInfo = (from ch in db.Child
                                         where ch.ClassId == cl.ClassId &&  ch.KindergartenId == value.KindergardenId
                                         select new
                                         {
                                             ch.ChildId,
                                             ch.ChildName,
                                             ch.Birthday,
                                             ch.Gender,
                                             planPhyExaInfoType = from p in db.PhysicalExaminationResults
                                                                  where p.PhysicalExaminationPlaneId == value.PlanPhyExmamintionId && p.ChildId == ch.ChildId
                                                                  select p,
                                         }
                            ),
                        };
                    responser.Attach = new
                    {
                        ClassAndChildDetail = classAndChildInfo.ToList(),
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

        private ApiResponser displayPhyNormal(PhysicalExamination value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var heigAgeNormal = from i in db.IndividualIndexDistributions
                                        group i by i.Age into ageId
                                        select new
                                        {
                                            key = ageId.Key
                                        };
                    responser.Attach = new
                    {
                        AgeType = heigAgeNormal.ToList()
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
        //
        // GET: /PhysicalExamination/
        [System.Web.Http.HttpPost]
        public ApiResponser post(PhysicalExamination value)
        {
            if (value.DataType == DataType.weightHeightDetail)
            {
                return weightHeightDetail(value);
            }
            if (value.DataType == DataType.individuvalStInfo)
            {
                return individualStInfo(value);
            }
            if (value.DataType == DataType.sendAddIndStDetail)
            {
                return addIndividualStDetail(value);
            }
            if (value.DataType == DataType.planPhyExaminaition)
            {
                return planPhyExaminationInfo(value);
            }
            if(value.DataType == DataType.planPhyExaminationInfo){
                return planPhyExaDataInfo(value);
            }
            if (value.DataType == DataType.planPhyChildInfo)
            {
                return planPhyChildInfo(value);
            }
            if (value.DataType == DataType.displayNormal)
            {
                return displayPhyNormal(value);
            }
            return null;
        }
	}
}