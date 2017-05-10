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
    public class IntelligenceQuestionController : BabyBusApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(IntelligenceQuestionController));

        private ApiResponser AddIntelligenceQuestion(IntelligenceQuestion value)
        {
            var responser = new ApiResponser(true);
            if (value == null)
            {
                responser.Attach = new
                {
                    Status = "fail",
                };
                return responser;
            }
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    System.DateTimeOffset dateTime = new System.DateTimeOffset();
                    dateTime = System.DateTimeOffset.Now;
                    value.CreateTime = dateTime;
                    db.IntelligenceQuestions.Add(value);
                    db.SaveChanges();

                    responser.Attach = new
                    {
                        Status = "success",
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
        public ApiResponser Post(IntelligenceQuestion value)
        {
            return AddIntelligenceQuestion(value);
        }
	}
}