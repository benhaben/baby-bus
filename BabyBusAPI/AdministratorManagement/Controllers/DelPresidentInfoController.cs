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
    public class DelPresidentInfoController : ApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(DelPresidentInfoController));

        public enum delType {
            delPresident=1,
            delClass=2,
            delKindergarten=3,
            classGraduate = 4
        }
        public class requestType {
            public delType type { set; get; }
            public int mubiaoId { set; get; }
        }

        private ApiResponser delPresident(requestType request)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {

                    User updateUser = db.Users.First(u => u.UserId == request.mubiaoId);
                    if (updateUser != null)
                    {
                        updateUser.Cancel = true;
                        updateUser.LoginName += "d";
                        db.SaveChanges();
                        responser.Attach = new
                        {
                            status = "success",
                        };
                        return responser;
                    }
                    else
                    {
                        responser.Attach = new
                        {
                            status = "fail",
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
        }//delPresident end


        private ApiResponser delClass(requestType request)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    Class updateClass = db.Classes.First(c => c.ClassId == request.mubiaoId);
                    if (updateClass != null)
                    {
                        updateClass.Cancel = true;
                        db.SaveChanges();
                        responser.Attach = new
                        {
                            status = "success",
                        };
                        return responser;
                    }
                    else
                    {
                        responser.Attach = new
                        {
                            status = "fail",
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
        }//delClass end

        private ApiResponser delKindergarten(requestType request)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    Kindergarten updateKin = db.Kindergarten.First(k => k.KindergartenId == request.mubiaoId);
                    if (updateKin != null)
                    {
                        updateKin.Cancel = true;
                        db.SaveChanges();
                        responser.Attach = new
                        {
                            status = "success",
                        };
                        return responser;
                    }
                    else
                    {
                        responser.Attach = new
                        {
                            status = "fail",
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
        }//delKindergarten end

        //班级毕业，改变班级信息以及classType的值
        private ApiResponser changeClassGraduate(requestType request)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    Class updateClass = db.Classes.First(c => c.ClassId == request.mubiaoId);
                    if (updateClass != null)
                    {
                        updateClass.ClassType = -1;
                        var year = DateTime.Today.Year;
                        var oldClsName = updateClass.ClassName;
                        var newClsName = year + "级" + oldClsName + "(已毕业)";
                        updateClass.ClassName = newClsName;

                        db.SaveChanges();
                        responser.Attach = new
                        {
                            status = "success",
                        };
                        return responser;
                    }
                    else
                    {
                        responser.Attach = new
                        {
                            status = "fail",
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
        }//delClass end

        [System.Web.Http.HttpPost]
        public ApiResponser post(requestType request)
        {
            //return delPresident(delUser);
            if (request.type == delType.delPresident)
            {
                return delPresident(request);
            }else if(request.type == delType.delClass)
            {
                return delClass(request);
            }else if(request.type == delType.delKindergarten)
            {
                return delKindergarten(request);
            }else if(request.type == delType.classGraduate){
                return changeClassGraduate(request);
            }
            return null;
        }//post End
	}
}