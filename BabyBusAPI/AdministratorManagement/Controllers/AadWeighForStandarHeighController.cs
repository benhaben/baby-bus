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
    public class AadWeighForStandarHeighController : ApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(AadWeighForStandarHeighController));

        private ApiResponser addWeighFors(HeightForStandardWeight value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    value.CreateTime = DateTime.Now;
                    db.HeightForStandardWeights.Add(value);
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
            }
            return responser;
        }
        //
        // GET: /AadWeighForStandarHeigh/
       [System.Web.Http.HttpPost]
        public ApiResponser post(HeightForStandardWeight value)
        {
            return addWeighFors(value);
        }
	}
}