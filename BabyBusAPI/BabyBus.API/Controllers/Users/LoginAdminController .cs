
using System;
using System.Web.Http;
using BabyBus.Core.Helper;
using BabyBus.Model.Entities.Login;
using BabyBus.Model.Entities.Login.Enum;
using BabyBus.Service.General;

namespace BabyBus.API.Controllers.Users
{
    public class LoginAdminController : ApiController
    {
        private readonly IAdminService _service;

        public LoginAdminController(IAdminService service)
        {
            _service = service;
        }

        // POST: api/LoginAdmin
        public ApiResponser Post(Admin admin) 
        {
            var response = new ApiResponser(true);

            try
            {
                LoginCheckStatus result = _service.CheckLoginAdmin(admin);
                switch (result)
                {
                    case LoginCheckStatus.UnCorrect:
                        response.Status = false;
                        response.Message = "用户名或密码错误";
                        break;
                    case LoginCheckStatus.Correct:
                        response.Status = true;
                        response.Attach = admin;
                        break;
                }
                response.Message = response.Message;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}