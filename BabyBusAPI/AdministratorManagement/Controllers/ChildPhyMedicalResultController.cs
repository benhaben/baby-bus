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
    public class ChildPhyMedicalResultController : ApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(ChildPhyMedicalResultController));

        private ApiResponser submitPhyInfo(PhysicalExaminationResult value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())

                {
                    value.CreateTime = DateTime.Now;
                    db.PhysicalExaminationResults.Add(value);
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

        //
        // GET: /ChildPhyMedicalResult/
        [System.Web.Http.HttpPost]
        public ApiResponser post(PhysicalExaminationResult value)
        {
            return submitPhyInfo(value);
        }
	}
}