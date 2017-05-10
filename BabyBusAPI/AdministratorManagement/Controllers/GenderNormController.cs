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
    
    public class GenderNormController : BabyBusApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(GenderNormController));

        private ApiResponser AddGenderNorm(GenderNorm value, GenderNormGroupAgeType typeValue)
        {
            var responser = new ApiResponser(true);
            if (value == null || typeValue == null)
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

                    typeValue.CreateTime = dateTime;
                    db.GenderNormGroupAgeTypes.Add(typeValue);
                    db.SaveChanges();

                    value.GenderNormGroupAgeTypeId = typeValue.GenderNormGroupAgeTypeId;
                    db.GenderNorms.Add(value);
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
        public ApiResponser Post(GenderNorm value, GenderNormGroupAgeType typeValue)
        {
            return null;
        }
	}
}