using System;
using System.Linq;
using System.Web.Http;
using BabyBus.Core.Helper;
using BabyBus.Model.Entities.Login;
using BabyBus.Model.Entities.Login.Enum;
using BabyBus.Service.General;
using BabyBus.Service.General.Login;
using BabyBus.Service.Models;
using log4net;

namespace BabyBus.API.Controllers.Users
{
    public class LoginController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof (LoginController));
        private readonly IUserService _service;
        private readonly ICheckoutService _checkoutService;

        public LoginController(IUserService service, ICheckoutService checkoutService) {
            _service = service;
            _checkoutService = checkoutService;
        }

        // POST: api/Login
        public ApiResponser Post(User loginUser) {
            var response = new ApiResponser(true);

            try
            {
                LoginCheckStatus result = _service.CheckLoginUser(loginUser);
                switch (result)
                {
                    case LoginCheckStatus.UnCorrect:
                        response.Status = false;
                        response.Message = "用户名或密码错误";
                        break;
                    case LoginCheckStatus.Correct:
                        response.Status = true;
                        User user = _service.GetUserByLoginName(loginUser.LoginName);
                        var userDetail = new UserDetailModel(user);
                        if (userDetail.RoleType == RoleType.Parent)
                        {
                            userDetail.Child = _service.GetChildByUserId(userDetail.UserId);
                            if (userDetail.Child != null)
                            {
                                userDetail.ChildId = userDetail.Child.ChildId;
                                userDetail.ClassId = userDetail.Child.ClassId;
                                userDetail.KindergartenId = userDetail.Child.KindergartenId;
                                if (userDetail.ClassId > 0)
                                    userDetail.Class = _service.GetClassById(userDetail.Child.ClassId);
                                userDetail.Kindergarten = _service.GetKindergartenById(userDetail.Child.KindergartenId);
                                userDetail.ImageName = userDetail.Child.ImageName;
                            }
                            else
                            {
                                var checkout = _checkoutService.GetAllCheckout()
                                    .Where(x => x.UserId == user.UserId)
                                    .OrderByDescending(x=>x.CheckOutId)
                                    .FirstOrDefault();
                                if (checkout != null)
                                {
                                    userDetail.IsCheckout = true;
                                    userDetail.CheckoutAuditType = checkout.AuditType;
                                    userDetail.CheckoutMemo = checkout.Memo;
                                }
                                else
                                {
                                    userDetail.IsCheckout = false;
                                }
                            }
                        }
                        if (userDetail.RoleType == RoleType.Teacher)
                        {
                            userDetail.Class = _service.GetClassById(userDetail.ClassId);
                            userDetail.Kindergarten = _service.GetKindergartenById(userDetail.KindergartenId);
                        }
                        if (userDetail.RoleType == RoleType.HeadMaster)
                        {
                            userDetail.Kindergarten = _service.GetKindergartenById(userDetail.KindergartenId);
                        }
                        response.Attach = userDetail;
                        break;
                }
                response.Message = response.Message;
                return response;
            }
            catch (Exception ex)
            {
                log.Debug(ex.ToString());
                response.Status = false;
                response.Message = ex.Message;
                return response;
            }
        }
    }
}