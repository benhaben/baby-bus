using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData.Query;
using BabyBus.Core.Helper;
using BabyBus.Model.Entities.Main;
using BabyBus.Service.General;
using BabyBus.Service.General.Main;

namespace BabyBus.API.Controllers.Common
{
    public class ClassController : ApiController
    {
        private readonly IClassService _service;

        public ClassController(IClassService service)
        {
            _service = service;
        }

        //POST: api/Class add & edit
        public ApiResponser Post(Class _class) 
        {
            var response = new ApiResponser(true);

            if (_class.ClassId != 0)
            {
                _service.EditClass(_class);
                response.Attach = _class;
            }
            else
            {
                _service.CreateClass(_class);
                response.Attach = _class;
            }
            return response;
        }

        //DELETE: api/Class delete
        //public ApiResponser Delete(int id)
        //{
        //    var response = new ApiResponser(true);
        //    var result = _service.GetClass(id);
        //    if (result != null )
        //    {
        //        _service.DeleteClass(id );
        //        response.Attach = result;
        //    }
        //    return response;        
        //}

        //GET: api/Class inquary
        public ApiResult<Class> Get(ODataQueryOptions<Class> options)
        {
            var list = _service.GetAllClass().AsQueryable();

            var result = options.ApplyTo(list);

            return new ApiResult<Class>
            {
                Status = true,
                Items = result as IEnumerable<Class>,
                TotalCount = list.Count()
            };
 

        }
    }
}
