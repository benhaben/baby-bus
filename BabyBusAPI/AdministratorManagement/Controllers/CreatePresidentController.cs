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
    public class CreatePresidentController : ApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(CreatePresidentController));
        private ApiResponser CreatePresidents(User value)
        {
            var responser = new ApiResponser(true);
            System.DateTimeOffset dateTime = new System.DateTimeOffset();
            dateTime = System.DateTimeOffset.Now;
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    value.WeChatInfoId = 0;
                    value.ClassId = 0;
                    value.ImageName = "";
                    value.Cancel = false;
                    value.CreateTime = dateTime;
                    value.OpenId = "";
                    var kinId = value.KindergartenId;
                    if (value.RoleType == 4)
                    {
                        value.KindergartenId = -1;    
                    }
                    db.Users.Add(value);
                    db.SaveChanges();

                    if (value.RoleType == 4)
                    {
                        var newUserKinder = new UserKindergartenRealation
                        {
                            UserId = value.UserId,
                            KindergartenId = kinId,
                        };
                        db.UserKindergartenRealations.Add(newUserKinder);
                        db.SaveChanges();
                    }
                   
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

        //
        // GET: /CreatePresident/
        [System.Web.Http.HttpPost]
        public ApiResponser post(User  value)
        {
            return CreatePresidents(value);
        }
	}
}