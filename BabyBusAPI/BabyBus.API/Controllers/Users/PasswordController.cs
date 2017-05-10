using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BabyBus.Core.Helper;
using BabyBus.Model.Entities.Login;
using BabyBus.Service.Models;
using BabyBus.Service.WebAPI;

namespace BabyBus.API.Controllers.Users
{
    public class PasswordController : ApiController
    {
        private readonly IMemberService _service;

        public PasswordController(IMemberService service) {
            _service = service;
        }

        //POST: api/Password
        public ApiResponser Post(UserDetailModel user) {
            var response = new ApiResponser();

            try
            {
                if (!_service.CheckPassword(user.UserId, user.OldPassword))
                {
                    response.Status = false;
                    response.Message = "原密码不正确";
                }
                else
                {
                    _service.ChangePassword(user.UserId, user.NewPassword);
                    response.Status = true;
                }
            }
            catch (Exception ex) {
                response.Status = false;
                response.Message = ex.Message;
                return response;
            }


            return response;
        }
    }
}
