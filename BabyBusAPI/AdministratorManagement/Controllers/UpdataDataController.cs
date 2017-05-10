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
    public class UpdataDataController : ApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(UpdataDataController));

        public enum DataType
        {
            createClasses = 0,
            checkClasses = 1,
            updataClass = 2,
            updataCheckClass = 3,
        }
        public class UpdataData
        {
            public DataType Types { get; set; }
            public string ClassName { get; set; }
            public int KindergartenId { get; set; }
            public int ClassId { get; set; }
            public int ClassType { get; set; }
        }
        private ApiResponser CheckClasses(UpdataData value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var classCounts = from c in db.Classes
                                      where c.KindergartenId == value.KindergartenId
                                      && c.ClassName == value.ClassName
                                      && c.Cancel == false
                                      select c;
                    var classes = classCounts.Count();
                    responser.Attach = new
                    {
                        Classes = classes,
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

        private ApiResponser UpdataCheckClasses(UpdataData values)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var checkCls = from c in db.Classes
                                   where c.ClassId != values.ClassId && c.KindergartenId == values.KindergartenId && c.ClassName == values.ClassName
                                   select c;
                    var checkClsCount = checkCls.Count();
                    responser.Attach = new
                    {
                        UpdataCheckClass = checkClsCount,
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
        private ApiResponser CreateClasses(UpdataData values)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var newClasses = new Class
                    {
                        KindergartenId = values.KindergartenId,
                        ClassName = values.ClassName,
                        Description = "",
                        ClassCount = 0,
                        Cancel = false,
                        ClassType = values.ClassType
                    };
                    db.Classes.Add(newClasses);
                    db.SaveChanges();
                    responser.Attach = new
                    {
                        Status = "Success"
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
        private ApiResponser UpdataClasses(UpdataData value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    Class updataCls = db.Classes.First(c => c.ClassId == value.ClassId);
                    updataCls.ClassName = value.ClassName;
                    updataCls.ClassType = value.ClassType;
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
        // GET: /UpdataData/
        [System.Web.Http.HttpPost]
        public ApiResponser post(UpdataData value)
        {
            if (value.Types == DataType.createClasses)
            {
                return CreateClasses(value);
            } if (value.Types == DataType.checkClasses)
            {
                return CheckClasses(value);
            }
            if (value.Types == DataType.updataClass)
            {
                return UpdataClasses(value);
            }
            if (value.Types == DataType.updataCheckClass)
            {
                return UpdataCheckClasses(value);
            }
            return null;
        }
    }
}