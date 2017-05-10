using BabyBus.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BabyBus.AutoModel;

namespace AdministratorManagement.Controllers
{
    public class AllKindergartensController : BabyBusApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(AllKindergartensController));

        private ApiResponser AllKindergartensInfo()
        {
            StartWatch();
            var responser = new ApiResponser(true);
            try
            {
                var db = new BabyBus_Entities();

                var kindergartensInfo = from k in db.Kindergarten
                                        orderby k.KindergartenId
                                        group k by new { k.KindergartenId, k.KindergartenName} into g
                                        select new
                                        {
                                            kindergartenId = g.Key.KindergartenId,
                                            kindergartenName = g.Key.KindergartenName,
                                        };

                var classCount = from c in db.Classes
                                 group c by c.KindergartenId into g
                                 select new 
                                 {
                                     kindergartenId = g.Key,
                                     classCount = g.Count()
                                 };


                responser.Attach = new
                {
                    KindergartensInfo = kindergartensInfo.ToList(),
                    classCount = classCount.ToList(),
                };

            }
            catch (Exception ex)
            {
                responser.Status = false;
                responser.Message = ex.Message;
                Log.Fatal(ex.Message, ex);
                return responser;
            }
            EndWatch();
            return responser;
        }


        [System.Web.Http.HttpPost]
        public ApiResponser Post()
        {
            return AllKindergartensInfo();
        }
	}
}