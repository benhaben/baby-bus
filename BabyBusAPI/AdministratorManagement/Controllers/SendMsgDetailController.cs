using BabyBus.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BabyBus.AutoModel;

namespace AdministratorManagement.Controllers
{
    public class SendMsgDetailController : BabyBusApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(SendMsgDetailController));
        public class SendMsgDetailRequest
        {
            public long NoticeId { get; set; }
        }


        private ApiResponser msgDetailInfo(SendMsgDetailRequest request)
        {
            StartWatch();
            var response = new ApiResponser(true);

            try
            {
                using (var db = new BabyBus_Entities())
                {

                    var noticeInfo = from n in db.Notice
                                     where n.NoticeId == request.NoticeId
                                     select new 
                                     {
                                         msg = n,
                                         year = n.CreateTime.Year,
                                         month = n.CreateTime.Month,
                                         day = n.CreateTime.Day,
                                         userName = from u in db.Users
                                                    where u.UserId == n.UserId
                                                    select u.RealName,
                                        images = n.NormalPics
                                     };

                    response.Attach = noticeInfo.ToList();

                }//using end
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


        [System.Web.Http.HttpPost]
        public ApiResponser Post(SendMsgDetailRequest request)
        {
            return msgDetailInfo(request);
        }


	}
}