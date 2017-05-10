using BabyBus.AutoModel;
using BabyBus.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdministratorManagement.Controllers
{
    public class ParameterConfigrationController : BabyBusApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(ParameterConfigrationController));

        //
        // GET: /ParameterConfigration/
        public enum testType
        {
            planeDetail = 1,
            parentTestQuestionDetail = 2,
            teachChildTestSorceQuestion = 3,
            teachIntelligenceAccessment = 4,
            parentIntelligenceAcc = 5,
        }
        public class DataDetail
        {
            public testType TestType { get; set; }
            public int QuestionType { get; set; }
            public int IntelligenceQuestionId { get; set; }
            public int UserId { get; set; }
            public string Sorce { get; set; }
            public int InAssType { get; set; }
        }
        private ApiResponser checkParameterConfigInfo(DataDetail value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                   var parameterInfoDetail = from ga in db.GenderNormGroupAgeTypes
                                              join gn in db.GenderNorms
                                              on ga.GenderNormGroupAgeTypeId equals gn.GenderNormGroupAgeTypeId
                                              group ga by ga.AgeType into g
                                              select new
                                              {
                                                  ageType = g.Key,
                                                  total = g.Count(),
                                                  allGenderNorms = from ga1 in db.GenderNormGroupAgeTypes
                                                                   join gn1 in db.GenderNorms
                                                                   on ga1.GenderNormGroupAgeTypeId equals gn1.GenderNormGroupAgeTypeId
                                                                   where ga1.AgeType == g.Key
                                                                   group ga1 by new { gn1.GenderNormGroupAgeTypeId,ga1.IsActive } into g1
                                                                   select new
                                                                   {
                                                                       checkNormal = g1.Key.IsActive,
                                                                       genderNormATId = g1.Key.GenderNormGroupAgeTypeId,
                                                                       genderNorms = from gnd2 in db.GenderNorms
                                                                                     where gnd2.GenderNormGroupAgeTypeId == g1.Key.GenderNormGroupAgeTypeId
                                                                                     group gnd2 by gnd2.ChildGender into g2
                                                                                     select new
                                                                                     {
                                                                                         gender = g2.Key,
                                                                                         details = from gnd3 in db.GenderNorms
                                                                                                   where gnd3.GenderNormGroupAgeTypeId == g1.Key.GenderNormGroupAgeTypeId
                                                                                                   && gnd3.ChildGender == g2.Key
                                                                                                   select gnd3,
                                                                                     },
                                                                   },

                                              };
                    

                    responser.Attach = new
                    {
                        ParameterInfoDetail = parameterInfoDetail.ToList(),
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

        private ApiResponser parentIntelligenceQuestionDetail(DataDetail value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var testQuestion = from i in db.IntelligenceQuestions
                                       where i.QuestionType == value.QuestionType && i.IsActive == true
                                       select i;
                    var chidInfo = from u in db.ParentChildRelation
                                   join c in db.Child on u.ChildId equals c.ChildId
                                   where u.UserId == value.UserId
                                   select new
                                   {
                                       c.ChildName,
                                   };
                    responser.Attach = new
                    {
                        TestQuestion = testQuestion.ToList(),
                        ChidInfo = chidInfo.ToList(),
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

        private ApiResponser teacherSorceTestQuestion(DataDetail value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var questionContent = from i in db.IntelligenceQuestions
                                          where i.IntelligenceQuestionId == value.IntelligenceQuestionId
                                          select i;
                    var teacherSorceChilInfo = from u in db.Users
                                               join c in db.Child on u.ClassId equals c.ClassId
                                               where u.UserId == value.UserId
                                               select new
                                               {
                                                   c.ChildName,
                                                   c.ChildId,
                                                   c.ImageName,
                                               };
                    responser.Attach = new
                    {
                        QuestionContent = questionContent.ToList(),
                        TeacherSorceChilInfo = teacherSorceChilInfo.ToList(),
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

        private ApiResponser teacherSorceTestInfo(DataDetail value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var teacherSorces = value.Sorce;
                    string[] sorceInfo = teacherSorces.Split(new Char[] { ';' });
                    for (var i = 0; i < sorceInfo.Length; i++)
                    {
                        var childSorce = sorceInfo[i];
                        var childIdVal = 0;
                        var sorces = 0;
                        if(childSorce == "" || childSorce == null){
                            continue;
                        }
                        string[] childIntelliAss = childSorce.Split(new Char[] { ',' });
                        for (var c = 0; c < childIntelliAss.Length; c++)
                        {
                            var childInAss = childIntelliAss[c];
                            if (childInAss == "" || childInAss == null)
                            {
                                continue;
                            }
                            var childInAssSorce = int.Parse(childInAss);
                            if (c == 0)
                            {
                                childIdVal = childInAssSorce;
                            }
                            if (c == 1)
                            {
                                sorces = childInAssSorce;
                            }
                        }
                        var newIntelligenceAssessment = new IntelligenceAssessment
                        {
                            ChildId = childIdVal,
                            CreateTime = System.DateTime.Now,
                            UserId = value.UserId,
                            IntelligenceAssessmentType = value.InAssType,
                            IntelligenceQuestionId = value.IntelligenceQuestionId,
                            Score = sorces,
                        };
                        db.IntelligenceAssessments.Add(newIntelligenceAssessment);
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

        private ApiResponser parentSorceIntelligenceSorce(DataDetail value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var userDetailInfo = from p in db.ParentChildRelation
                                         where p.UserId == value.UserId
                                         select new{
                                            p.ChildId,
                                         };

                    var childIdInfo = userDetailInfo.ToList();
                    var childIdVal = childIdInfo[0].ChildId;


                    string[] parentQuestion = value.Sorce.Split(new Char[] { ';' });
                    for (var i = 0; i < parentQuestion.Length; i++)
                    {
                        var sorce = 0;
                        var intelligenceQusetionValId = 0;
                        var parentTestSorce = parentQuestion[i];
                        if (parentTestSorce == "" || parentTestSorce == null)
                        {
                            continue;
                        }
                        string[] InAccSorce = parentTestSorce.Split(new Char[] { ',' });
                        for (var ia = 0; ia < InAccSorce.Length; ia++)
                        {
                            var inAccSorceInfo = InAccSorce[ia];
                            if (inAccSorceInfo == "" || inAccSorceInfo == null)
                            {
                                continue;
                            }
                            var inAssSorceData = int.Parse(inAccSorceInfo);
                            if (ia == 0)
                            {
                                intelligenceQusetionValId = inAssSorceData;
                            }
                            if (ia == 1)
                            {
                                sorce = inAssSorceData;
                            }
                        }
                        var parentInAssSorceData = new IntelligenceAssessment
                        {
                            ChildId = childIdVal,
                            CreateTime = System.DateTime.Now,
                            UserId = value.UserId,
                            IntelligenceAssessmentType = value.InAssType,
                            IntelligenceQuestionId = intelligenceQusetionValId,
                            Score = sorce,
                        };
                        db.IntelligenceAssessments.Add(parentInAssSorceData);

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

        [System.Web.Http.HttpPost]

        public ApiResponser post(DataDetail value)
        {
            if (value.TestType == testType.planeDetail)
            {
                return checkParameterConfigInfo(value);
            }
            if (value.TestType == testType.parentTestQuestionDetail)
            {
                return parentIntelligenceQuestionDetail(value);
            }
            if (value.TestType == testType.teachChildTestSorceQuestion)
            {
                return teacherSorceTestQuestion(value);
            }
            if (value.TestType == testType.teachIntelligenceAccessment)
            {
                return teacherSorceTestInfo(value);
            }
            if (value.TestType == testType.parentIntelligenceAcc)
            {
                return parentSorceIntelligenceSorce(value);
            }
            return null;
        }
      
	}
}