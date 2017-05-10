using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BabyBus.Core.Helper;
using BabyBus.Model.Entities.Login;
using BabyBus.Service.WebAPI;

namespace BabyBus.API.Controllers.Users
{
    public class RegisterController : ApiController
    {
        private readonly IMemberService _service; 
         
        public RegisterController(IMemberService service)
        {
            _service = service;
        }

        public ApiResponser Post(User createUser)
        {
            var response = new ApiResponser();

            if (_service.CheckLoginName(createUser.LoginName))
            {
                response.Status = false;
                response.Message = "该用户名已被注册";
                return response;
            } 
            try
            {
                response.Attach = _service.Register(createUser);
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
                return response;
            }
        }
    }
}
