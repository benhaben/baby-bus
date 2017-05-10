using System;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using BabyBus.AutoModel;
using BabyBus.Core.Helper;
using System.Web.SessionState;



namespace AdministratorManagement.Controllers
{
    //[Authorize]
    public class UserController : BabyBusApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(UserController));
        public class UserRequest
        {
            public int KindergartenId { get; set; }

            public string KindergartenName { get; set; }

            public int ClassId { get; set; }

            public int Role { get; set; }
            public SelectType Type { get; set; }
            public int ChildId { get; set; }
            public int ClassType { get; set; }
        }

        public enum SelectType
        {
            Kindergarten = 0,
            classChild = 1,
            childInformations = 2,
            editTeacherInfo = 3,
			displays = 4,
        }

        private ApiResponser Kindergartens(UserRequest value)
        {
            StartWatch("in to Kindergartens");

            var response = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    if (value.Role == 0)
                    {
                        var kindergartens = (from i in db.Kindergarten
                                             orderby i.KindergartenId
                                             select new { KindergartenId = i.KindergartenId, KindergartenName = i.KindergartenName }).Distinct();
                        response.Attach = new
                        {
                            Kindergartens = kindergartens.ToList()
                        };

                    }
                    else if (value.Role == 1)
                    {
                        var kindergartens = (from i in db.Kindergarten
                                             where i.KindergartenId == value.KindergartenId
                                             orderby i.KindergartenId
                                             select new { KindergartenId = value.KindergartenId, KindergartenName = i.KindergartenName }).Distinct();
                        response.Attach = new
                        {
                            Kindergartens = kindergartens.ToList()
                        };
                    }
                    else if (value.Role == 2)
                    {
                        var kindergartens = (from i in db.Kindergarten
                                             where i.KindergartenId == value.KindergartenId
                                             orderby i.KindergartenId
                                             select new { KindergartenId = value.KindergartenId, KindergartenName = i.KindergartenName }).Distinct();
                        var aa = kindergartens.Count();
                        response.Attach = new
                        {
                            Kindergartens = kindergartens.ToList()
                        };
                    }
                    else
                    {
                        response.Attach = "无权限";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
                Log.Fatal(ex.Message, ex);
                return response;
            }
            EndWatch();
            return response;
        }





        private ApiResponser classAndChildInfo(UserRequest value)
        {
            StartWatch("in to classAndChildInfo");
            var response = new ApiResponser(true);
            try
            {
				System.DateTimeOffset dateTime = new System.DateTimeOffset();
                dateTime = System.DateTimeOffset.Now;
                var timeYear = dateTime.Year;
                var timeMouth = dateTime.Month;
                using (var db = new BabyBus_Entities())
                {

                    if (value.Role == 0 || value.Role == 1)
                    {
                        var classAndChildInfo = from c in db.Classes
                                                where c.KindergartenId == value.KindergartenId
                                                && c.Cancel == false
                                                && c.ClassType == value.ClassType
                                                orderby c.ClassType descending
                                                orderby c.ClassId
                                                select new
                                                {
                                                    classInfo = new
                                                    {
                                                        classId = c.ClassId,
                                                        className = c.ClassName,
                                                        classCount = c.ClassCount,
                                                        classType = c.ClassType
                                                    },
                                                    childInfos = (from cd in db.Child
                                                                  where cd.ClassId == c.ClassId
                                                                  && cd.KindergartenId == value.KindergartenId
                                                                  select new
                                                                  {
                                                                      childId = cd.ChildId,
                                                                      childName = cd.ChildName,
                                                                      classId = cd.ClassId,
                                                                      kinId = cd.KindergartenId,
                                                                      image = cd.ImageName,
                                                                      birthDays = cd.Birthday,
                                                                      genderInfo = cd.Gender,
                                                                      parentInfo = (
                                                                        from p in db.ParentChildRelation
                                                                        join u in db.Users
                                                                        on p.UserId equals u.UserId
                                                                        where p.ChildId == cd.ChildId
                                                                        && u.Cancel == false
                                                                        select new
                                                                        {
                                                                            u.RealName,
                                                                            u.LoginName,
                                                                            u.UserId,
                                                                            u.Phone
                                                                        }
  																	  ),
                                                                      childPayType = (from cp in db.ChildPays
                                                                                      where cp.CreatDate.Year == timeYear && cp.CreatDate.Month == timeMouth && cp.ChildId == cd.ChildId
                                                                                      select cp
                                                                      )
                                                                  })
                                                };
                        response.Attach = new
                        {
                            classAndChildInfo = classAndChildInfo.ToList(),
                        };
                    }
                    else if (value.Role == 2)
                    {
                        var classAndChildInfo = from c in db.Classes
                                                where c.KindergartenId == value.KindergartenId
                                                && c.ClassId == value.ClassId
                                                && c.Cancel == false
                                                orderby c.ClassType descending
                                                orderby c.ClassId
                                                select new
                                                {
                                                    classInfo = new
                                                    {
                                                        classId = c.ClassId,
                                                        className = c.ClassName,
                                                        classCount = c.ClassCount,
                                                        classType = c.ClassType
                                                    },
                                                    childInfos = (from cd in db.Child
                                                                  where cd.ClassId == c.ClassId
                                                                  && cd.KindergartenId == value.KindergartenId
                                                                  select new
                                                                  {
                                                                      childId = cd.ChildId,
                                                                      childName = cd.ChildName,
                                                                      classId = cd.ClassId,
                                                                      kinId = cd.KindergartenId,
                                                                      parentInfo = (
                                                                       from p in db.ParentChildRelation
                                                                       join u in db.Users
                                                                       on p.UserId equals u.UserId
                                                                       where p.ChildId == cd.ChildId
                                                                       && u.Cancel == false
                                                                       select new
                                                                       {
                                                                           u.RealName,
                                                                           u.LoginName,
                                                                           u.UserId,
                                                                           u.Phone
                                                                       }
                                                                     ),
                                                                      childPayType = (from cp in db.ChildPays
                                                                                      where cp.CreatDate.Year == timeYear && cp.CreatDate.Month == timeMouth && cp.ChildId == cd.ChildId
                                                                                      select cp
                                                                       ),
                                                                  }),
                                                };
                        
                        response.Attach = new
                        {
                            classAndChildInfo = classAndChildInfo.ToList(),
                        };
                    }

                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
                Log.Fatal(ex.Message, ex);
                return response;
            }
            EndWatch();
            return response;
        }

        private ApiResponser childInfo(UserRequest value)
        {
            StartWatch("in to childInfo");

            var response = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    if (value.ChildId != 0 && value.ClassId != 0 && value.KindergartenId != 0)
                    {

                        var childInfo = from cd in db.Child
                                        where cd.ChildId == value.ChildId
                                        && cd.ClassId == value.ClassId
                                        && cd.KindergartenId == value.KindergartenId
                                        select new
                                        {
                                            ChildId = cd.ChildId,
                                            ChildName = cd.ChildName,
                                            Gender = cd.Gender,
                                            KindergartenName = from k in db.Kindergarten
                                                               where k.KindergartenId == cd.KindergartenId
                                                               select k.KindergartenName,
                                            ClassName = from cl in db.Classes
                                                        where cl.ClassId == cd.ClassId
                                                        select cl.ClassName,
                                            ClassId = cd.ClassId,
                                            KindergartenId = cd.KindergartenId
                                        };

                        var parent = from u in db.Users
                                     join p in db.ParentChildRelation
                                     on u.UserId equals p.UserId
                                     join c in db.Child
                                     on p.ChildId equals c.ChildId
                                     where c.ChildId == value.ChildId
                                     && c.KindergartenId == value.KindergartenId
                                     && c.ClassId == value.ClassId
                                     && u.Cancel == false
                                     select new
                                     {
                                         u.RealName,
                                         u.LoginName,
                                         u.UserId,
                                         u.Phone
                                     };

                        var teacher = from u in db.Users
                                      join c in db.Classes
                                      on u.ClassId equals c.ClassId
                                      join cd in db.Child
                                      on c.ClassId equals cd.ClassId
                                      where cd.ChildId == value.ChildId
                                      && cd.KindergartenId == value.KindergartenId
                                      && cd.ClassId == value.ClassId
                                      && u.RoleType == 2
                                      select new
                                      {
                                          u.RealName,
                                          u.LoginName,
                                          u.UserId,
                                          u.Phone
                                      };

                        var generateUserId = from uk in db.UserKindergartenRealations
                                             where uk.KindergartenId == value.KindergartenId
                                             select uk.UserId;


                        var president = from u in db.Users
                                        where (u.KindergartenId == value.KindergartenId
                                        || generateUserId.Contains(u.UserId))
                                        && (new int?[] { 3, 4 }).Contains(u.RoleType)
                                        && u.Cancel == false
                                        select new
                                        {
                                            u.RealName,
                                            u.LoginName,
                                            u.UserId,
                                            u.Phone
                                        };

                        response.Attach = new
                        {
                            Parent = parent.ToList(),
                            Teacher = teacher.ToList(),
                            ChildInfo = childInfo.ToList(),
                            President = president.ToList(),
                        };
                    }
                    else
                    {
                        response.Attach = "失败";
                    }
                }
            }
            catch (Exception e)
            {
                response.Status = false;
                response.Message = e.Message;
                Log.Fatal(e.Message, e);
                return response;
            }
            EndWatch();
            return response;
        }

        private ApiResponser editTeacherInfo(UserRequest value)
        {
            StartWatch("in to tacherInfo");

            var response = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    if (value.ClassId != 0 && value.KindergartenId != 0)
                    {
                        var teacher = from u in db.Users
                                      where u.ClassId == value.ClassId
                                      && u.KindergartenId == value.KindergartenId
                                      && u.RoleType == 2
                                      select u;

                        response.Attach = new
                        {
                            TeacherInfo = teacher.ToList(),
                        };
                    }
                    else
                    {
                        response.Attach = "失败";
                    }
                }
            }
            catch (Exception e)
            {
                response.Status = false;
                response.Message = e.Message;
                Log.Fatal(e.Message, e);
                return response;
            }
            EndWatch();
            return response;
        }

   private ApiResponser DisClsData(UserRequest value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var calssIds = from c in db.Classes
                                   where c.KindergartenId == value.KindergartenId
                                   orderby c.ClassType descending
                                   orderby c.ClassId
                                   select c;
                    responser.Attach = new
                    {
                        ClassIdDisplays = calssIds.ToList(),
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
        //Post: api/NoticeHomework
        public ApiResponser Post(UserRequest value)
        {
            StartWatch("in to Post: api/User");
            if (value.Type == SelectType.Kindergarten)
            {
                return Kindergartens(value);
            }
            else if (value.Type == SelectType.classChild)
            {
                return classAndChildInfo(value);
            }
            else if (value.Type == SelectType.childInformations)
            {
                return childInfo(value);
            }
            else if (value.Type == SelectType.editTeacherInfo)
            {
                return this.editTeacherInfo(value);
			}else if(value.Type == SelectType.displays){
                return DisClsData(value);
            }
            else
            {
                return new ApiResponser()
                {
                    Status = false,
                    Message = "SelectType is wrong",
                    Attach = value
                };
            }
        }


    }
}