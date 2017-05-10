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
    public class CreateKindergartenController : ApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(CreateKindergartenController));


        private ApiResponser checkKindergartenIsUnique(Kindergarten kin)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var kinInfo = from k in db.Kindergarten
                                  where k.Cancel == false
                                  && k.KindergartenName == kin.KindergartenName
                                  select k;
                    if (kinInfo != null && kinInfo.Count() > 0)
                    {
                        responser.Attach = new
                        {
                            status = "chongfu",
                        };
                        return responser;
                    }
                    else 
                    {
                        kin.Cancel = false;
                        kin.KindergartenCount = 0;
                        db.Kindergarten.Add(kin);
                        db.SaveChanges();

                        //默认添加一个删除班级
                        Class classInfo = new Class() { 
                            ClassName = "删除",
                            KindergartenId = kin.KindergartenId,
                            ClassType = -1,
                        };
                        db.Classes.Add(classInfo);
                        db.SaveChanges();

                        responser.Attach = new
                        {
                            status = "success",
                        };
                        return responser;
                    }
                    
                }//using end
            }
            catch (Exception e)
            {
                responser.Status = false;
                responser.Message = e.Message;
                Log.Fatal(e.Message, e);
                return responser;
            }
        }//checkKindergartenIsUnique end

        /*private Class generateDeleteClassInfo() { 
            Class cls = new Class();
            cls.ClassName = "删除";
            
            return cls;
        }*/


        [System.Web.Http.HttpPost]
        public ApiResponser post(Kindergarten kin)
        {
            return checkKindergartenIsUnique(kin);
        }//post End
	}
}