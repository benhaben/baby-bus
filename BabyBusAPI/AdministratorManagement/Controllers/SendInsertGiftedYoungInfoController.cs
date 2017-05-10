using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BabyBus.AutoModel;
using BabyBus.Core.Helper;

namespace AdministratorManagement.Controllers
{
    public class SendInsertGiftedYoungInfoController : ApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(SendInsertGiftedYoungInfoController));
        private ApiResponser submitGiftedYoungInfo(Notice value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    value.CreateTime = System.DateTimeOffset.Now;
                    db.Notice.Add(value);
                    db.SaveChanges();
                    responser.Attach = new 
                    {
                        Status = "successful",
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
        // GET: /SendInsertGiftedYoungInfo/
    [System.Web.Http.HttpPost]
        public ApiResponser post(Notice value)
        {
            return submitGiftedYoungInfo(value);
        }
	}
}