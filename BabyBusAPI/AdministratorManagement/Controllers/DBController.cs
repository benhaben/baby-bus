
using BabyBus.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using BabyBus.AutoModel;
using System.Web.Mvc;
//using System.

namespace AdministratorManagement.Controllers
{
    public class DBController : ApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(DBController));
        public enum DBType
        {
            Creat = 0,
            Update = 1,
            ChangeClass = 2,
            CreatUser = 3,
            Check = 4,
            UpdateParentUser = 5,
            CheckParentsUser = 6,
            CreateTeachersUser = 7,
            CheckTeacherUser = 8,
            UpdateTeacherUser = 9,
            CheckUpdateTeacherUser = 10,
            Teacher = 11,
            CheckUserData = 12,
            resertpassword = 13,
        }
        public enum GenderType
        {
            Boy = 1,  //男
            Girl = 0  //女
        }
        public class DB
        {
            public DBType DBType { get; set; }
            public string ChildName { get; set; }
            public string ClassName { get; set; }
            public int ChildId { get; set; }
            public int ClassId { get; set; }
            public int KindergartenId { get; set; }
            public int ClassIdChoos { get; set; }
            public int Gender { get; set; }
            public string LoginName { get; set; }
            public string RealName { get; set; }
            public string Password { get; set; }
            public int UserId { get; set; }
            public int RoleType { get; set; }
            public string Phone { get; set; }
            public System.DateTimeOffset Birthday { get; set; }
        }


        private ApiResponser CheckUserData(DB value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    if (value.RoleType == 1)
                    {
                        var checkParents = from u in db.Users
                                           join p in db.ParentChildRelation on u.UserId equals p.UserId
                                           join cn in db.Child on p.ChildId equals cn.ChildId
                                           join cl in db.Classes on cn.ClassId equals cl.ClassId
                                           join k in db.Kindergarten on cl.KindergartenId equals k.KindergartenId
                                           where u.UserId == value.UserId
                                           select new
                                           {
                                               u.UserId,
                                               k.KindergartenName,
                                               cl.ClassName,
                                               cn.ChildName,
                                               u.Phone
                                           };
                        responser.Attach = new
                        {
                            CheckParents = checkParents.ToList(),
                        };
                    }
                    else if (value.RoleType == 2)
                    {
                        var checkTeachers = from u in db.Users
                                            join k in db.Kindergarten on u.KindergartenId equals k.KindergartenId
                                            join cl in db.Classes on u.ClassId equals cl.ClassId
                                            where u.UserId == value.UserId
                                            select new
                                            {
                                                u.UserId,
                                                k.KindergartenName,
                                                u.RealName,
                                                cl.ClassName,
                                                u.Phone
                                            };
                        responser.Attach = new
                        {
                            CheckTeachers = checkTeachers.ToList(),
                        };
                    }
                    else if (value.RoleType == 3)
                    {
                        var checkTittlePresident = from u in db.Users
                                                   join k in db.Kindergarten on u.KindergartenId equals k.KindergartenId
                                                   where u.UserId == value.UserId
                                                   select new
                                                   {
                                                       u.UserId,
                                                       k.KindergartenName,
                                                       u.RealName,
                                                       u.Phone
                                                   };
                        responser.Attach = new
                        {
                            CheckTittlePresident = checkTittlePresident.ToList(),
                        };
                    }
                    else if (value.RoleType == 4)
                    {
                        var checkPres = from u in db.Users
                                        join uk in db.UserKindergartenRealations on u.UserId equals uk.UserId
                                        join k in db.Kindergarten on uk.KindergartenId equals k.KindergartenId
                                        where u.UserId == value.UserId
                                        select new
                                        {
                                            u.RealName,
                                            k.KindergartenName,
                                            u.Phone
                                        };
                        responser.Attach = new
                        {
                            CheckPresidents = checkPres.ToList(),
                        };
                    }
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

        private ApiResponser CheckParentUser(DB value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var checkUpdateParent = from u in db.Users
                                            where u.UserId != value.UserId && u.Cancel == false && u.LoginName == value.LoginName
                                            select u;
                    var parentUpdate = checkUpdateParent.Count();
                    responser.Attach = new
                    {
                        CheckUpdateParent = parentUpdate,
                        CheckParentData = checkUpdateParent.ToList(),
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

        private ApiResponser CheckUser(DB value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var checkLoginName = from u in db.Users where u.LoginName == value.LoginName && u.Cancel == false select u;
                    var loginName = checkLoginName.Count();
                    responser.Attach = new
                    {
                        CheckUserLoginName = loginName,
                        CheckParentData = checkLoginName.ToList(),
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
        private ApiResponser CheckUpdateTeacherUsers(DB value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var checkUpdateTeachers = from u in db.Users
                                              where u.UserId != value.UserId && u.LoginName == value.LoginName && u.Cancel != true
                                              select u;
                    var teachersUpdate = checkUpdateTeachers.Count();
                    responser.Attach = new
                    {
                        CheckUpdateTeacher = teachersUpdate,
                        TeacherDatas = checkUpdateTeachers.ToList(),
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

        private ApiResponser CreateUserData(DB value)
        {
            System.DateTimeOffset dateTime = new System.DateTimeOffset();
            dateTime = System.DateTimeOffset.Now;
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var newUser = new User
                    {
                        WeChatInfoId = 0,
                        LoginName = value.LoginName,
                        RealName = value.RealName,
                        Password = value.Password,
                        RoleType = 1,
                        KindergartenId = value.KindergartenId,
                        ClassId = value.ClassId,
                        ImageName = "",
                        Cancel = false,
                        CreateTime = dateTime,
                        OpenId = "",
                        Phone = value.Phone
                    };
                    db.Users.Add(newUser);
                    db.SaveChanges();
                    var newParentChildRelation = new ParentChildRelation
                    {
                        ChildId = value.ChildId,
                        UserId = newUser.UserId
                    };

                    db.ParentChildRelation.Add(newParentChildRelation);
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

        private ApiResponser CreateTeacherUserData(DB value)
        {
            var responser = new ApiResponser(true);
            try
            {
                System.DateTimeOffset dateTime = new System.DateTimeOffset();
                dateTime = System.DateTimeOffset.Now;
                using (var db = new BabyBus_Entities())
                {
                    var newUser = new User
                    {
                        WeChatInfoId = 0,
                        LoginName = value.LoginName,
                        RealName = value.RealName,
                        Password = value.Password,
                        RoleType = 2,
                        KindergartenId = value.KindergartenId,
                        ClassId = value.ClassIdChoos,
                        ImageName = "",
                        Cancel = false,
                        CreateTime = dateTime,
                        OpenId = "",
                        Phone = value.Phone
                    };
                    db.Users.Add(newUser);
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

        private ApiResponser UpdateTeacherUserData(DB value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    User newTeacherUser = db.Users.First(u => u.UserId == value.UserId);
                    if (value.RoleType == 0 || value.RoleType == 3 || value.RoleType == 4)
                    {
                        newTeacherUser.Phone = value.Phone;
                        newTeacherUser.LoginName = value.LoginName;
                        newTeacherUser.RealName = value.RealName;
                        newTeacherUser.ClassId = value.ClassIdChoos;
                    }
                    else
                    {
                        newTeacherUser.Phone = value.Phone;
                        newTeacherUser.RealName = value.RealName;
                        newTeacherUser.ClassId = value.ClassIdChoos;
                    }
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
        private ApiResponser ChangeClass(DB value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var className = (from cl in db.Classes
                                     where cl.KindergartenId == value.KindergartenId
                                     && cl.Cancel == false
                                     select new { cl.ClassName, cl.ClassId }).Distinct();
                    responser.Attach = new
                    {
                        ClassName = className.ToList()
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

        private ApiResponser TeacherDetail(DB value)
        {
            var responer = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var teacherData = from u in db.Users
                                      where u.UserId == value.UserId
                                      select u;
                    var classIds = from c in db.Classes
                                   where c.KindergartenId == value.KindergartenId
                                   && c.Cancel == false
                                   select c;
                    responer.Attach = new
                    {
                        TeacherDatas = teacherData.ToList(),
                        ClassIds = classIds.ToList(),
                    };
                }
            }
            catch (Exception e)
            {
                responer.Status = false;
                responer.Message = e.Message;
                Log.Fatal(e.Message, e);
                return responer;
            }
            return responer;
        }
        private ApiResponser CreateDate(DB value)
        {
            System.DateTimeOffset datetimeO = new System.DateTimeOffset();
            datetimeO = System.DateTimeOffset.Now;

            var responser = new ApiResponser(true);
            if (value.ClassIdChoos != -1 && value.ChildName != null)
            {
                try
                {
                    using (var db = new BabyBus_Entities())
                    {
                        var newChild = new Child
                        {
                            KindergartenId = value.KindergartenId,
                            ClassId = value.ClassIdChoos,
                            CardPasswordId = 0,
                            ChildName = value.ChildName,
                            Birthday = value.Birthday,
                            CreateTime = datetimeO,
                            ImageName = "",
                            Cancel = false,
                            Gender = value.Gender,
                            IsMember = false
                        };
                        db.Child.Add(newChild);
                        var classes = from cl in db.Classes where cl.ClassId == value.ClassIdChoos select cl;
                        foreach (var clCount in classes)
                        {
                            clCount.ClassCount += 1;
                        }
                        // db.Child.Intersect();
                        db.SaveChanges();
                        responser.Attach = new
                        {
                            ChildIdS = newChild.ChildId,
                            States = "Success"
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

            }
            else
            {
                responser.Attach = new
                {
                    States = "fail",
                };
            }
            return responser;
        }
        private ApiResponser UpdateDate(DB value)
        {
            //StartWatch();
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var updateClass = from cl in db.Classes
                                      where cl.ClassId == value.ClassIdChoos
                                      && cl.Cancel == false
                                      select cl;
                    var classId = from cl in db.Classes
                                  where cl.ClassId == value.ClassId
                                  && cl.Cancel == false
                                  select cl;
                    foreach (var cc in classId)
                    {
                        cc.ClassCount -= 1;
                    }
                    foreach (var p in updateClass)
                    {
                        p.ClassCount += 1;
                    }
                    Child cust = db.Child.First(c => c.ChildId == value.ChildId);
                    cust.ChildName = value.ChildName;
                    cust.ClassId = value.ClassIdChoos;
                    cust.KindergartenId = value.KindergartenId;
                    cust.Gender = value.Gender;
                    db.SaveChanges();
                    responser.Attach = new
                    {
                        Success = "success",
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

            //EndWatch();
            return responser;
        }

        private ApiResponser UpdateParentsUser(DB value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    User updateParentUsers = db.Users.First(u => u.UserId == value.UserId);
                    if (value.RoleType == 0 || value.RoleType == 3 || value.RoleType == 4)
                    {
                        updateParentUsers.Phone = value.Phone;
                        updateParentUsers.RealName = value.RealName;
                        updateParentUsers.LoginName = value.LoginName;

                    }
                    else
                    {
                        updateParentUsers.Phone = value.Phone;
                        updateParentUsers.RealName = value.RealName;
                    }
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
        private ApiResponser resertPasswordInfo(DB value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    User resert = db.Users.First(u => u.UserId == value.UserId);
                    resert.Password = "123456";
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
                return responser;
            }
            return responser;
        }
        //
        // GET: /DB/
        [System.Web.Http.HttpPost]
        public ApiResponser post(DB value)
        {
            if (value.DBType == DBType.Update)
            {
                return UpdateDate(value);
            }
            if (value.DBType == DBType.ChangeClass)
            {
                return ChangeClass(value);
            }
            if (value.DBType == DBType.Creat)
            {
                return CreateDate(value);
            }
            if (value.DBType == DBType.Check)
            {
                return CheckUser(value);
            }
            if (value.DBType == DBType.CreatUser)
            {
                return CreateUserData(value);
            }
            if (value.DBType == DBType.UpdateParentUser)
            {
                return UpdateParentsUser(value);
            } if (value.DBType == DBType.CheckParentsUser)
            {
                return CheckParentUser(value);
            } if (value.DBType == DBType.CreateTeachersUser)
            {
                return CreateTeacherUserData(value);
            } if (value.DBType == DBType.UpdateTeacherUser)
            {
                return UpdateTeacherUserData(value);
            } if (value.DBType == DBType.CheckUpdateTeacherUser)
            {
                return CheckUpdateTeacherUsers(value);
            } if (value.DBType == DBType.Teacher)
            {
                return TeacherDetail(value);
            } if (value.DBType == DBType.CheckUserData)
            {
                return CheckUserData(value);
            }
            if (value.DBType == DBType.resertpassword)
            {
                return resertPasswordInfo(value);
            }
            return null;
        }
    }
}