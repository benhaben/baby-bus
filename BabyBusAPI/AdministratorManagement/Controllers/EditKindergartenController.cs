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
    public class EditKindergartenController : ApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(EditKindergartenController));

        public enum ActionType {
            kindInfo = 0,
            editKind = 1,
            checkKindInfo = 2,
        }
        public class KinderData
        {
            public ActionType ActionType { get; set; }
            public int KindergartenId { get; set; }
            public string KindergartenName { get; set; }
            public string City { get; set; }
            public string Description { get; set; }
        }
        private ApiResponser KindergarteInfo(KinderData value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var kindergarInfo = from k in db.Kindergarten
                                        where k.KindergartenId == value.KindergartenId
                                        select k;
                    responser.Attach = new
                    {
                        KindergarInfo = kindergarInfo.ToList(),
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

        private ApiResponser ChecktKindData(KinderData value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var checkKind = from k in db.Kindergarten
                                    where k.KindergartenName == value.KindergartenName && k.KindergartenId != value.KindergartenId
                                    select k;
                    var kindCount = checkKind.Count();
                    responser.Attach = new
                    {
                        CheckKind = kindCount,
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

        private ApiResponser EditKindData(KinderData value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    Kindergarten editKind = db.Kindergarten.First(k => k.KindergartenId == value.KindergartenId);
                    editKind.KindergartenName = value.KindergartenName;
                    editKind.City = value.City;
                    editKind.Description = value.Description;
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
        //
        // GET: /EditKindergarten/
        [System.Web.Http.HttpPost]
         public ApiResponser post(KinderData value) {
             if (value.ActionType == ActionType.kindInfo)
             {
                 return KindergarteInfo(value);
             }
             if (value.ActionType == ActionType.editKind)
             {
                 return EditKindData(value);
             }
             if (value.ActionType == ActionType.checkKindInfo)
             {
                 return ChecktKindData(value);
             }
             return null;
        }
	}
}