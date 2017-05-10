using System;
using System.Linq;
using System.Web.Http;
using System.Web.OData.Query;
using BabyBus.API.Utils;
using BabyBus.Core.Helper;
using BabyBus.Model.Entities.Login;
using BabyBus.Model.Entities.Login.Enum;
using BabyBus.Service.General;
using BabyBus.Service.General.Login;
using BabyBus.Service.General.Main;
using BabyBus.Service.Models;

namespace BabyBus.API.Controllers.Users {
    public class CheckoutController : ApiController {
        private readonly ICheckoutService checkoutService;
        private readonly IClassService classService;
        private readonly IUserService userService;

        public CheckoutController(ICheckoutService checkoutService, IClassService classService, IUserService userService) {
            this.checkoutService = checkoutService;
            this.classService = classService;
            this.userService = userService;
        }

        //Get:Api/Checkout
        public ApiResult<CheckoutModel> Get(ODataQueryOptions<Checkout> options) {
            var prelist = checkoutService.GetAllCheckout();
            prelist = options.ApplyTo(prelist) as IQueryable<Checkout>;

            IQueryable<CheckoutModel> list = from co in prelist
                join u in userService.GetAllUser() on co.UserId equals u.UserId
                join c in classService.GetAllClass() on co.ClassId equals c.ClassId
                orderby co.AuditType 
                select new CheckoutModel {
                    CheckOutId = co.CheckOutId,
                    AuditType = co.AuditType,
                    RoleType = co.RoleType,
                    UserId = co.UserId,
                    RealName = co.RealName,
                    VerifyCode = co.VerifyCode,
                    City = co.City,
                    KindergartenId = co.KindergartenId,
                    ClassId = co.ClassId,
                    ChildId = co.ChildId,
                    ChildName = co.ChildName,
                    Gender = co.Gender,
                    Birthday = co.Birthday,
                    CreateTime = co.CreateTime,
                    ImageName = co.ImageName,
                    Memo = co.Memo,
                    LoginName = u.LoginName,
                    ClassName = c.ClassName,
                };
            var enumerable = list.ToList();
            var result = new ApiResult<CheckoutModel> {
                Status = true,
                Items = enumerable,
                TotalCount = enumerable.Count
            };
            return result;
        }

        //Post:Api/Checkout
        public ApiResponser Post(CheckoutModel model) {
            var response = new ApiResponser(true);
            try {
                switch (model.AuditType) {
                    case AuditType.Pending:
                        GetImageName(model);
                        checkoutService.CreateCheckout(model, ref response);
                        response.Attach = model.ImageName;
                        break;
                    case AuditType.Passed:
                        checkoutService.Approve(model.CheckOutId, ref response);
                        break;
                    case AuditType.Refused:
                        checkoutService.Refuse(model.CheckOutId, model.Memo);
                        break;
                    default:
                        response.Status = false;
                        response.Message = "审批有误，请联系客服";
                        break;
                }
            }
            catch (Exception ex) {
                response.Status = false;
                response.Message = ex.ToString();
            }

            return response;
        }

        private void GetImageName(CheckoutModel checkModel) {
            if (checkModel.HasHeadImage) {
                var guid = Guid.NewGuid();
                var fileName = guid + Config.PhotoSuffix;
                checkModel.ImageName = fileName;
            }
        }
    }
}