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
    public class DeleteDataController : ApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(DeleteDataController));
        public enum DeleteType
        {
            deleteChild = 0,
            deleteTeacher = 1,
        }
        public class DeleteDate
        {
            public DeleteType DeleteType { get; set; }
            public int ClassId { get; set; }
            public int KindergartenId { get; set; }
            public int ChildId { get; set; }
            public int UserId { get; set; }
        }

        private ApiResponser DeleteChild(DeleteDate value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    Class changeClassCount = db.Classes.First(c => c.ClassId == value.ClassId);
                    Class deleteChild = db.Classes.First(c => c.KindergartenId == value.KindergartenId && c.ClassName == "删除");
                    Child changeChildClassId = db.Child.First(c => c.ChildId == value.ChildId);
                    changeChildClassId.ClassId = deleteChild.ClassId;
                    deleteChild.ClassCount += 1;
                    changeClassCount.ClassCount -= 1;
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
                responser.Message = "没有建立名为“删除”的班级！"; ;
                Log.Fatal(e.Message, e);
                return responser;
            }
            return responser;
        }

        private ApiResponser DeleteTeacher(DeleteDate value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var kind = value.KindergartenId;
                    var kinds = value.UserId;
                    Class deleteClass = db.Classes.First(c => c.KindergartenId == value.KindergartenId && c.ClassName == "删除");
                    User deleteTeacher = db.Users.First(u => u.UserId == value.UserId);
                    deleteTeacher.ClassId = deleteClass.ClassId;
                    db.SaveChanges();
                    responser.Attach = new
                    {
                        DeleteTeacherStatus = "success",
                    };
                }
            }
            catch (Exception e)
            {
                responser.Status = false;
                responser.Message = "没有建立名为“删除”的班级！";
                Log.Fatal(e.Message, e);
                return responser;
            }
            return responser;
        }
        //
        // GET: /DeleteData/
       
        [System.Web.Http.HttpPost]
        public ApiResponser post(DeleteDate value)
        {
            if (value.DeleteType == DeleteType.deleteChild)
            {
                return DeleteChild(value);
            }
            if (value.DeleteType == DeleteType.deleteTeacher)
            {
                return DeleteTeacher(value);
            }
            return null;
        }
	}
}