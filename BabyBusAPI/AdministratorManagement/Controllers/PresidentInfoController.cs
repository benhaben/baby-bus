using System;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using BabyBus.AutoModel;
using BabyBus.Core.Helper;
using System.Web.SessionState;

namespace AdministratorManagement.Controllers
{
    public class PresidentInfoController : BabyBusApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(PresidentInfoController));


        public class PresidentRequest
        {
            public int KindergartenId { get; set; }
        }

        private ApiResponser presidentInfo(PresidentRequest value)
        {
            StartWatch("in to Kindergartens");

            var response = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {

                    var generateUserId = from uk in db.UserKindergartenRealations
                                        where uk.KindergartenId == value.KindergartenId
                                        select uk.UserId;


                    var president = from u in db.Users
                                    where (u.KindergartenId == value.KindergartenId
                                    || generateUserId.Contains(u.UserId))
                                    && (new int?[]{3,4}).Contains(u.RoleType)
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
                        President = president.ToList(),
                    };
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
        }// presidentInfo end




        [System.Web.Http.HttpPost]
        //Post: api/NoticeHomework
        public ApiResponser Post(PresidentRequest value)
        {
            return presidentInfo(value);
        }//Post end
	}
}