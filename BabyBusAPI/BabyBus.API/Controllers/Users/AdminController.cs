using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BabyBus.Core.Helper;
using BabyBus.Model.Entities.Login;
using BabyBus.Model.Entities.Login.Enum;
using BabyBus.Service.General;
using System.Web.OData.Query;

namespace BabyBus.API.Controllers.Users
{
    public class AdminController : ApiController
    {
        private readonly IAdminService _service;

        public AdminController(IAdminService service)
        {
            _service = service;
        }

        // POST: api/Admin add & edit
        public ApiResponser Post(Admin admin) 
        {
            var response = new ApiResponser(true);
            if (admin.AdminId != 0)
            {
                _service.EditAdmin(admin);
                response.Attach = admin;
            }
            else
            {
                _service.CreateAdmin(admin);
                response.Attach = admin;
            }
            return response;
        }
        // GET: api/Admin inquery
        public ApiResult<Admin> Get(ODataQueryOptions<Admin> options)
        {
            var response = new ApiResult<Admin>();
            var list = _service.GerAllAdmins().AsQueryable();
            var result = options.ApplyTo(list) as IEnumerable<Admin>;

            if (result != null)
            {
                var enumerable = result.ToList();
                response.Status = true;
                response.Items = enumerable;
                response.TotalCount = enumerable.Count;
            }
            else
            {
                response.Status = false;
                response.Message = "查询期间发生错误";
            }
            return response;

        }
    }
}